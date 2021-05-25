using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing : Room
{
	protected int stairwayBreadth = 2;

	public Landing()
	{
		widthRange = new IntRange(10, 10);
		heightRange = new IntRange(10, 10);
	}


	public Direction disabledDoorDirection;

	public Vector2Int stairwayUp; 
	public Vector2Int stairwayDown;

	public virtual void SetupRoom(Vector2Int stairway, Verticality verticality, Direction disabledDoorDirection, int story)
	{
		this.story = story;
		SetRoomDimensions();
		this.disabledDoorDirection = disabledDoorDirection;

		x = Random.Range(stairway.x - width + stairwayBreadth, stairway.x - stairwayBreadth);
		y = Random.Range(stairway.y - height + stairwayBreadth, stairway.y - stairwayBreadth);

		if (verticality == Verticality.Down)
		{
			stairwayUp = stairway;
			stairwayDown = new Vector2Int(Random.Range(x, x + width - stairwayBreadth), Random.Range(y, y + height) - stairwayBreadth);
		}
		else
		{
			stairwayDown = stairway;
			stairwayUp = new Vector2Int(Random.Range(x, x + width - stairwayBreadth), Random.Range(y, y + height) - stairwayBreadth);
		}
	}


	public override bool TestRoomValidity(Board board)
	{
		x = Mathf.Clamp(x, board.boardMargin, board.columns - board.boardMargin - width);
		y = Mathf.Clamp(y, board.boardMargin, board.rows - board.boardMargin - height);
		return true;
	}

	public override Doorway PossibleDoorway()
	{
		Direction desiredDirection = FindDesiredDoorDirection(new List<Direction>() { disabledDoorDirection });

		switch (desiredDirection)
		{
			case Direction.North:
				return new Doorway(x + width / 2, y + height - 1, Direction.North);
			case Direction.South:
				return new Doorway(x + width / 2, y, Direction.South);
			case Direction.East:
				return new Doorway(x + width - 1, y + height / 2, Direction.East);
			default:
				return new Doorway(x, y + height / 2, Direction.West);
		}
	}
}
