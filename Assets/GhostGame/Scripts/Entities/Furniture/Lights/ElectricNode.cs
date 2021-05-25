using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricNode : Furniture
{

	[SyncVar(hook = nameof(UpdateElectricEffects))]
	private bool switchIsOn = true;

	public virtual void FlipOn()
	{
		bool oldValue = switchIsOn;
		switchIsOn = true;
		UpdateElectricEffects(oldValue, switchIsOn);
	}
	public virtual void FlipOff()
	{
		bool oldValue = switchIsOn;
		switchIsOn = false;
		UpdateElectricEffects(oldValue, switchIsOn);
	}
	public virtual void FlipReverse()
	{
		bool oldValue = switchIsOn;
		switchIsOn = !switchIsOn;
		UpdateElectricEffects(oldValue, switchIsOn);

	}

	protected ElectricNode parentNode = null;
	/*public ElectricNode ParentNode
	{
		get => parentNode;
		set
		{
			parentNode = value;
			UpdateElectricEffects(switchIsOn, switchIsOn);
		}
	}*/

	protected List<ElectricNode> children = new List<ElectricNode>();
	public List<ElectricNode> Children { get => children; }

	protected List<bool> childEnabled = new List<bool>();
	public List<bool> ChildEnabled { get => childEnabled; }


	public bool IsOn
	{
		get
		{
			if (switchIsOn)
			{
				if (parentNode == null || parentNode.IsSupplyingChildPower(this)) {
					return true;
				}
			}
			return false;
		}
	}
	protected virtual void UpdateElectricEffects(bool oldValue, bool newValue)
	{
		foreach(ElectricNode child in children)
		{
			child.UpdateElectricEffects(oldValue, newValue);
		}
		// To be overriden, called on client when switch is changed
	}

	public void AddChild(ElectricNode node, bool enabled = true)
	{
		children.Add(node);
		childEnabled.Add(enabled);
		node.parentNode = this;
		node.UpdateElectricEffects(node.switchIsOn, node.switchIsOn);
	}
	public void RemoveChild(ElectricNode node)
	{
		int index = children.IndexOf(node);
		RemoveChild(index);
	}
	public void RemoveChild(int index)
	{
		children.RemoveAt(index);
		childEnabled.RemoveAt(index);
	}

	public void DisableChild(ElectricNode node)
	{
		int index = children.IndexOf(node);
		DisableChild(index);
	}
	public void DisableChild(int index)
	{
		childEnabled[index] = false;
	}

	public void EnableChild(ElectricNode node)
	{
		int index = children.IndexOf(node);
		EnableChild(index);
	}
	public void EnableChild(int index)
	{
		childEnabled[index] = true;
	}

	public void SwitchChild(ElectricNode node)
	{
		int index = children.IndexOf(node);
		EnableChild(index);
	}
	public void SwitchChild(int index)
	{
		childEnabled[index] = !childEnabled[index];
	}

	public bool IsSupplyingChildPower(ElectricNode node)
	{
		if (switchIsOn)
		{
			if (parentNode == null || parentNode.IsSupplyingChildPower(this))
			{
				int index = children.IndexOf(node);
				if (childEnabled[index] == true)
				{
					return true;
				}
			}
		}
		return false;
	}

	

}
