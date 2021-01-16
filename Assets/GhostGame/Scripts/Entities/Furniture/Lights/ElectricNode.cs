using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricNode : Entity
{

	private bool switchOn = true;
	public bool SwitchOn { get => switchOn; }

	public virtual void FlipOn()
	{
		switchOn = true;
	}
	public virtual void FlipOff()
	{
		switchOn = false;
	}
	public virtual void FlipReverse()
	{
		switchOn = !switchOn;
	}

	public ElectricNode parentNode = null;

	public List<ElectricNode> children = new List<ElectricNode>();
	public List<ElectricNode> Children { get => children; }

	public List<bool> childEnabled = new List<bool>();
	public List<bool> ChildEnabled { get => childEnabled; }


	public bool IsOn
	{
		get
		{
			if (switchOn)
			{
				if (parentNode == null || parentNode.IsSupplyingChildPower(this)) {
					return true;
				}
			}
			return false;
		}
	}

	public void AddChild(ElectricNode node, bool enabled = true)
	{
		children.Add(node);
		childEnabled.Add(enabled);
		node.parentNode = this;
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
		if (parentNode == null || parentNode.IsSupplyingChildPower(this))
		{
			int index = children.IndexOf(node);
			if (childEnabled[index] == true)
			{
				return true;
			}
		}
		return false;
	}

	

}
