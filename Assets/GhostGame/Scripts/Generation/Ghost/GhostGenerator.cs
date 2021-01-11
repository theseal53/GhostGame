using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGenerator
{

	public float slowSpeed = 1;
	public float mediumSpeed = 3;
	public float fastSpeed = 10;

	public Ghost GenerateGhost(Board board)
	{
		Ghost ghost = Object.Instantiate(PrefabRegistry.I.ghost).GetComponent<Ghost>();
		ghost.speed = RandomSpeed();
		ghost.summonRoom = RandomRoom(board);
		ghost.summonPosition = FindSummonPosition(ghost.summonRoom);
		ghost.summonCondition = RandomSummonCondition(ghost);

		return ghost;
	}

	private float RandomSpeed()
	{
		int rng = Random.Range(0, 3);
		switch(rng)
		{
			case 0:
				return slowSpeed;
			case 1:
				return mediumSpeed;
			default:
				return fastSpeed;
		}
	}
	private Room RandomRoom(Board board)
	{
		int rng = Random.Range(0, board.rooms.Count);
		return board.rooms[rng];
	}
	private Vector2 FindSummonPosition(Room room)
	{
		return new Vector2(room.x + room.width / 2, room.y + room.height / 2);
	}

	private GhostSummonCondition RandomSummonCondition(Ghost ghost)
	{
		return new TimeoutSummonCondition(ghost);
	}
}
