using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacter : Mob
{
	public float runSpeed = 40f;

	float horizontalMove = 0f;
	float verticalMove = 0f;

	bool interactPressed;
	bool itemPressed;

	public float moveSpeed = 10;

	public float interactRadius = 2;

	public Item heldItem;

	// Update is called once per frame
	void Update()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;

		interactPressed = Input.GetKeyDown(KeyCode.E);
		if (interactPressed)
		{
			AttemptInteraction();
		}

		itemPressed = Input.GetKeyDown(KeyCode.Q);
		if (itemPressed)
		{
			AttemptItemUse();
		}
	}

	void FixedUpdate()
	{
		// Move our character
		Vector2 move = new Vector2(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);
		Move(move * moveSpeed);

	}

	void AttemptInteraction()
	{
		Collider2D[] proximityObjects = Physics2D.OverlapCircleAll(transform.position, interactRadius);
		HashSet<Entity> availableEntities = new HashSet<Entity>();
		foreach(Collider2D collider in proximityObjects)
		{
			Entity entity = collider.GetComponent<Entity>();
			if (entity != null)
			{
				if (entity.IsInteractable())
				{
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

	public void AttemptItemUse()
	{
		if (heldItem != null)
		{
			heldItem.UseByCharacter();
		}
	}

}