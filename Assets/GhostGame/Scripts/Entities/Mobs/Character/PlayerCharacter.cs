using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;

public class PlayerCharacter : Mob
{
	[Header("Visibility")]
	public Light2D visibleLight;
	public Light2D lineOfSightLight;

	public InteractionRadius interactionRadius;

	public SortingGroup sortingGroup;

	[Header("Movement/Interaction")]
	public float moveSpeed = 10;

	float horizontalMove = 0f;
	float verticalMove = 0f;

	public Item heldItem;

	Entity primaryFocusTarget;

	// Update is called once per frame

	public override void OnStartAuthority()
	{
		visibleLight.gameObject.SetActive(true);
		lineOfSightLight.gameObject.SetActive(true);
		interactionRadius.OnRadiusEnter += OnInteractionRadiusEnter;
		interactionRadius.OnRadiusExit += OnInteractionRadiusExit;
		base.OnStopAuthority();
	}

	void Update()
	{
		if (hasAuthority)
		{
			CmdSetMove(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			bool interactPressed = Input.GetKeyDown(KeyCode.E);
			if (interactPressed)
			{
				CmdAttemptInteraction();
			}

			bool itemPressed = Input.GetKeyDown(KeyCode.Q);
			if (itemPressed)
			{
				CmdAttemptItemUse();
			}
		}
	}

	[ServerCallback]
	void FixedUpdate()
	{
		// Move our character
		Vector2 move = new Vector2(horizontalMove * moveSpeed * Time.fixedDeltaTime, verticalMove * moveSpeed * Time.fixedDeltaTime);
		Move(move);
		UpdatePrimaryTarget();		
	}

	void UpdatePrimaryTarget()
	{
		primaryFocusTarget = ChooseInteractionEntity(interactionRadius.colliderList);
		foreach (Entity entity in interactionRadius.colliderList)
		{
			entity.NotifyIsPrimaryTarget(entity == primaryFocusTarget);
		}
	}

	Entity ChooseInteractionEntity(List<Entity> availableEntities)
	{
		Entity bestOption = null;
		if (availableEntities.Count > 0)
		{
			foreach (Entity entity in availableEntities)
			{
				if (entity.IsInteractable() && entity != this)
				{
					bestOption = entity;
					break;
				}
			}
		}
		return bestOption;
	}

	[Command]
	void CmdSetMove(float horizontal, float vertical)
	{
		horizontalMove = horizontal;
		verticalMove = vertical;
	}

	[Command]
	void CmdAttemptInteraction()
	{
		if (primaryFocusTarget != null && primaryFocusTarget.IsInteractable())
		{
			print("Interacting");
			primaryFocusTarget.Interact(this);
		}
	}


	public void PickupItem(Item item)
	{
		item.gameObject.transform.parent = gameObject.transform;
		heldItem = item;
		heldItem.holdingCharacter = this;
		item.gameObject.transform.position = transform.position;
	}

	public void DropItem()
	{
		if (heldItem != null)
		{
			heldItem.gameObject.transform.parent = transform.parent;
			heldItem.holdingCharacter = null;
			heldItem = null;
		}
	}

	[Command]
	public void CmdAttemptItemUse()
	{
		if (heldItem != null)
		{
			heldItem.UseByCharacter();
		}
	}

	public override void ChangeStory(Verticality verticality)
	{
		base.ChangeStory(verticality);
		print("Changing to story " + storyLocation);
		EventHub.PlayerChangeStoryBroadcast(this);
	}

	protected override void UpdateLayering()
	{
		base.UpdateLayering();
		sortingGroup.sortingLayerID = Layering.StoryToSortingLayerID(storyLocation);
		sortingGroup.sortingOrder = Constants.ENTITY_SORTING_ORDER;
		visibleLight.m_ApplyToSortingLayers = new int[] { Layering.StoryToSortingLayerID(storyLocation) };
		interactionRadius.gameObject.layer = (int)Layering.StoryToPhysicsLayer(storyLocation);
	}

	void OnInteractionRadiusEnter(Collider2D other)
	{
		Entity otherEntity = other.GetComponent<Entity>();
		otherEntity?.NotifyInteractionRadiusChange(true);
	}

	void OnInteractionRadiusExit(Collider2D other)
	{
		Entity otherEntity = other.GetComponent<Entity>();
		otherEntity?.NotifyInteractionRadiusChange(false);
	}

}