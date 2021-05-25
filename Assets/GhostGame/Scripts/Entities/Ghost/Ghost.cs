//flowchart: https://lucid.app/lucidchart/3c61e7a5-26d0-4134-bbd4-1e2229ef7dd8/edit?beaconFlowId=00E91D9F05890CE6&page=0_0#

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static XoloStateMachine;

public class Ghost : Mob
{
    public NameSet nameSet;

    public Room summonRoom;
    public Vector2 summonPosition;

	public bool banishConditionInstalled = false;
	public bool appeaseConditionInstalled = false;

	public List<GhostTransitionCondition> summonTransitionConditions;
	public List<GhostTransitionCondition> generalTransitionConditions;
	public List<GhostTransitionCondition> passiveTransitionConditions;
	public List<GhostTransitionCondition> aggroTransitionConditions;

	XoloStateMachine stateMachine;

	public override void Init(int storyLocation)
	{
		stateMachine = new XoloStateMachine();
		stateMachine.PushState(DormantState);
		summonTransitionConditions = new List<GhostTransitionCondition>();
		generalTransitionConditions = new List<GhostTransitionCondition>();
		passiveTransitionConditions = new List<GhostTransitionCondition>();
		aggroTransitionConditions = new List<GhostTransitionCondition>();
		base.Init(storyLocation);
	}

	void FixedUpdate()
	{
		stateMachine.ExecuteState();
		// Move our character
		//Vector2 move = new Vector2(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);
		//Move(move * moveSpeed);
	}

/////////////////////////////////States
	public void DormantState()
	{
		foreach(GhostTransitionCondition condition in summonTransitionConditions)
		{
			State suggestedState = condition.CheckConditionTransition();
			if (suggestedState != null && suggestedState != stateMachine.CurrentState)
			{
				stateMachine.PopState();
				stateMachine.PushState(suggestedState);
				return;
			}
		}
	}

	public void SummonTransitionState()
	{
		Summon();
		stateMachine.PopState();
		stateMachine.PushState(PassiveState);
	}

	public void PassiveState()
	{
		foreach (GhostTransitionCondition condition in generalTransitionConditions)
		{
			State suggestedState = condition.CheckConditionTransition();
			if (suggestedState != null && suggestedState != stateMachine.CurrentState)
			{
				stateMachine.PopState();
				stateMachine.PushState(suggestedState);
				return;
			}
		}
		foreach (GhostTransitionCondition condition in passiveTransitionConditions)
		{
			State suggestedState = condition.CheckConditionTransition();
			if (suggestedState != null && suggestedState != stateMachine.CurrentState)
			{
				stateMachine.PopState();
				stateMachine.PushState(suggestedState);
				return;
			}
		}
	}

	public void AggroState()
	{
		foreach (GhostTransitionCondition condition in generalTransitionConditions)
		{
			State suggestedState = condition.CheckConditionTransition();
			if (suggestedState != null && suggestedState != stateMachine.CurrentState)
			{
				stateMachine.PopState();
				stateMachine.PushState(suggestedState);
				return;
			}
		}
		foreach (GhostTransitionCondition condition in aggroTransitionConditions)
		{
			State suggestedState = condition.CheckConditionTransition();
			if (suggestedState != null && suggestedState != stateMachine.CurrentState)
			{
				stateMachine.PopState();
				stateMachine.PushState(suggestedState);
				return;
			}
		}
	}

	public void AppeasedTransitionState()
	{
		print("I am appeased");
	}

	public void BanishedTransitionState()
	{
		print("I am banished");
	}

/////////Other functions


	public void Summon()
	{
		print("I am summoned!");
		transform.position = summonPosition;
	}

	public void Banish()
	{
		print("I am banished!");
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Returns true if enough personality has been installed to make for a complete ghost
	/// </summary>
	public bool PersonalityIsComplete()
	{
		if (nameSet != null &&
			summonTransitionConditions.Count > 0 &&
			generalTransitionConditions.Count > 0 &&
			passiveTransitionConditions.Count > 0 &&
			aggroTransitionConditions.Count > 0 &&
			banishConditionInstalled &&
			appeaseConditionInstalled)
		{
			return true;
		}
		return false;
	}
}
