using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static XoloStateMachine;

public class TimeoutTC : GhostTransitionCondition
{
	float timeToTrigger;
	public bool correctTimePassed = false;

	public TimeoutTC(Ghost ghost, State nextState, float timeToTrigger): base(ghost, nextState) {
		this.timeToTrigger = timeToTrigger;
	}

	protected override void SetupConditionListeners()
	{
		EventHub.GameTimeChange += CheckTime;
	}

	protected override void RemoveConditionListeners()
	{
		EventHub.GameTimeChange -= CheckTime;
	}
	
	private void CheckTime(float remainingTime)
	{
		if (timeToTrigger >= remainingTime)
		{
			correctTimePassed = true;
			RemoveConditionListeners();
		}
	}

	public override State CheckConditionTransition()
	{
		if (correctTimePassed)
			return nextState;
		return null;
	}
}
