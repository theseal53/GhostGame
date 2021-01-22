using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameLight : Entity
{

    private Light2D light;

	public bool isOn = true;

	public void Awake()
	{
		light = GetComponent<Light2D>();
	}

	public virtual void TurnOn()
	{
		light.enabled = true;
	}

	public virtual void TurnOff()
	{
		light.enabled = false;
	}


}
