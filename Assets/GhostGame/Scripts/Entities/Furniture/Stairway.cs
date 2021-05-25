using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairway : Furniture
{

	public Verticality verticality;

	public override bool IsInteractable()
	{
		return true;
	}

	public override void Interact(PlayerCharacter playerCharacter)
	{
		playerCharacter.ChangeStory(verticality);
		playerCharacter.JumpToPosition(transform.position);
	}
}
