﻿using System.Collections;
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

	protected override void Start()
	{
		base.Start();
		SetTargetSortingLayers();
	}

	private void SetTargetSortingLayers()
	{
		light2D.m_ApplyToSortingLayers = new int[] { Layering.StoryToSortingLayerID(storyLocation) };
	}

	protected override void UpdateElectricEffects(bool oldValue, bool newValue)
	{
		if (IsOn)
			light2D.enabled = true;
		else
			light2D.enabled = false;
		base.UpdateElectricEffects(oldValue, newValue);
	}
}
