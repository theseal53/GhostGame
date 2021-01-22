using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Mob
{
    public NameSet NameSet;
    public float speed;

    public Room summonRoom;
    public Vector2 summonPosition;

	public GhostSummonCondition summonCondition;

	void Update()
	{

	}

	void FixedUpdate()
	{
		// Move our character
		//Vector2 move = new Vector2(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);
		//Move(move * moveSpeed);
	}
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
}
