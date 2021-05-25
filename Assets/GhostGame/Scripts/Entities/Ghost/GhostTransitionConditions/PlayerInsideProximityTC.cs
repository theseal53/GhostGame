using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static XoloStateMachine;

public class PlayerInsideProximityTC : GhostTransitionCondition
{
	public PlayerInsideProximityTC(Ghost ghost, State nextState) : base(ghost, nextState) { }

	public override State CheckConditionTransition()
	{
		if (Time.deltaTime % 2 == 0)
		{
			return nextState;
		}
		return null;
	}
}
