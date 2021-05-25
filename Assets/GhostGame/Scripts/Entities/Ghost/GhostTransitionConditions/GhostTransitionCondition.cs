using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static XoloStateMachine;

public abstract class GhostTransitionCondition
{

	protected Ghost ghost;
	protected State nextState;
    public GhostTransitionCondition(Ghost ghost, State nextState)
	{
		this.ghost = ghost;
		SetupConditionListeners();
		this.nextState = nextState;
	}

	public void Deactivate()
	{
		RemoveConditionListeners();
	}

	protected virtual void SetupConditionListeners() { }

	protected virtual void RemoveConditionListeners() { }

	protected virtual void SummonGhost()
	{
		ghost.Summon();
		Deactivate();
	}

	public abstract State CheckConditionTransition();
}
