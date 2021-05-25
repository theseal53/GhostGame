using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : ElectricNode
{
	public override bool IsInteractable()
	{
		return true;
	}
	public override void Interact(PlayerCharacter playerCharacter)
	{
		FlipReverse();
	}
}
