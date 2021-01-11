using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
	public PlayerCharacter holdingCharacter;

	public override bool IsInteractable()
	{
		if (holdingCharacter == null)
		{
			return true;
		}
		return false;
	}

	public override void Interact(PlayerCharacter playerCharacter)
	{
		playerCharacter.PickupItem(this);
	}

	public void Activate()
	{
		gameObject.SetActive(true);
	}

	public virtual void UseByCharacter()
	{
		//To be overriden
	}
}
