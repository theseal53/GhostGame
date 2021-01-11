using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeoutSummonCondition : GhostSummonCondition
{
	public float timeRemainingForSummon = 55;

	public TimeoutSummonCondition(Ghost ghost): base(ghost)
	{
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
		if (timeRemainingForSummon >= remainingTime)
		{
			SummonGhost();
		}
	}
}
