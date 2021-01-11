using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public virtual bool IsInteractable()
	{
		return false;
	}
	public virtual void Interact(PlayerCharacter playerCharacter)
	{
		//To be overriden
	}
}
