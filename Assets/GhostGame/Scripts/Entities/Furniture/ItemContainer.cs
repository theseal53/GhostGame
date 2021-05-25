using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : Furniture
{
    public Item containedItem;

	public override bool IsInteractable()
	{
		return true;
	}

	public override void Interact(PlayerCharacter playerCharacter)
	{
		ReleaseItem(playerCharacter);
	}

	public void ReleaseItem()
	{
		containedItem.Activate();
	}
	public void ReleaseItem(PlayerCharacter playerCharacter)
	{
		containedItem.Activate();
		playerCharacter.PickupItem(containedItem);
		containedItem = null;
	}
}
