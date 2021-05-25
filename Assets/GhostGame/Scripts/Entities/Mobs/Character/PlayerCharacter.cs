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

	public SortingGroup sortingGroup;

	[Header("Movement/Interaction")]
	public float moveSpeed = 10;
	public float interactRadius = 2;

	float horizontalMove = 0f;
	float verticalMove = 0f;

	public Item heldItem;

	// Update is called once per frame

	public override void OnStartAuthority()
	{
		visibleLight.gameObject.SetActive(true);
		lineOfSightLight.gameObject.SetActive(true);
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
		Collider2D[] proximityObjects = Physics2D.OverlapCircleAll(transform.position, interactRadius, 1 << (int)Layering.StoryToPhysicsLayer(storyLocation));
		HashSet<Entity> availableEntities = new HashSet<Entity>();
		foreach(Collider2D collider in proximityObjects)
		{
			Entity entity = collider.GetComponent<Entity>();
			if (entity != null)
			{
				

				if (entity.IsInteractable())
				{
					print(entity.gameObject);
					availableEntities.Add(entity);
				}
			}
		}
		if (availableEntities.Count == 0)
		{
			return;
		}
		else
		{
			Entity targetEntity = ChooseInteractionEntity(availableEntities);
			targetEntity.Interact(this);
		}
	}

	Entity ChooseInteractionEntity(HashSet<Entity> availableEntities)
	{
		return availableEntities.First();
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
	}

}