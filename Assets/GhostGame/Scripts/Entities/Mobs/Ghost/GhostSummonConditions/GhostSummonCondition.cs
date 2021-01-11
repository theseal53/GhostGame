using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostSummonCondition
{

	Ghost ghost;
    public GhostSummonCondition(Ghost ghost)
	{
		this.ghost = ghost;
		SetupConditionListeners();
	}

	public void Deactivate()
	{
		RemoveConditionListeners();
	}

	protected abstract void SetupConditionListeners();

	protected abstract void RemoveConditionListeners();

	protected virtual void SummonGhost()
	{
		ghost.Summon();
		Deactivate();
	}
}
