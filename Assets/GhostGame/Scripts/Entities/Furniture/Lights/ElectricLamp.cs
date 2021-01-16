using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ElectricLamp : ElectricNode
{
	[HideInInspector]
	private Light2D light2D;

	public void Awake()
	{
		light2D = GetComponent<Light2D>();
		if (IsOn)
		{
			light2D.enabled = true;
		}
	}

	public override void FlipOn()
	{
		base.FlipOn();
		if (IsOn)
		{
			light2D.enabled = true;
		}
	}
	public override void FlipOff()
	{
		base.FlipOff();
		light2D.enabled = false;
	}
	public override void FlipReverse()
	{
		base.FlipReverse();
		if (IsOn)
		{
			light2D.enabled = true;
		} else
		{
			light2D.enabled = false;
		}
	}

	public override bool IsInteractable()
	{
		return true;
	}

	public override void Interact(PlayerCharacter playerCharacter)
	{
		FlipReverse();
	}
}
