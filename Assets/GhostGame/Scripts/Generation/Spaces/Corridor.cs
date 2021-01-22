//Taken largely from https://www.youtube.com/watch?v=wnoLaui3uO4

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Corridor
{
	public Doorway door1;
	public Doorway door2;
	public int length;
	public int breadth;
	public short id;

	public IntRange lengthRange = new IntRange(0, 5);

	public void SetupCorridor(Doorway doorway, Board board, short corridorId, int breadth = Constants.DEFAULT_DOOR_BREADTH)
	{
		door1 = doorway;
		this.breadth = breadth;
		id = corridorId;

		length = lengthRange.Random;

		//length = Mathf.Clamp(length, 1, maxLength);

		switch (doorway.roomOutDirection)
		{
			case Direction.North:
				door2 = new Doorway(door1.x, door1.y + length + 1, Direction.South, breadth);
				break;
			case Direction.East:
				door2 = new Doorway(door1.x + length + 1, door1.y, Direction.West, breadth);
				break;
			case Direction.South:
				door2 = new Doorway(door1.x, door1.y - length - 1, Direction.North, breadth);
				break;
			case Direction.West:
				door2 = new Doorway(door1.x - length - 1, door1.y, Direction.East, breadth);
				break;
		}
		door2.corridor = this;
	}

	public void PrintToTilesArray(short[,] tiles)
	{
		int stepX = 0;
		if (door2.x > door1.x)
		{
			stepX = 1;
		} else if (door2.x < door1.x)
		{
			stepX = -1;
		}

		int stepY = 0;
		if (door2.y > door1.y)
		{
			stepY = 1;
		}
		else if (door2.y < door1.y)
		{
			stepY = -1;
		}
		int currentX = door1.x + stepX;
		int currentY = door1.y + stepY;

		while (currentX != door2.x)
		{
			for (int i = 0; i < breadth; i++)
			{
				tiles[currentY+i,currentX] = id;
			}
			currentX += stepX;
		}
		while (currentY != door2.y)
		{
			for (int i = 0; i < breadth; i++)
			{
				tiles[currentY,currentX+i] = id;
			}
			currentY += stepY;
		}
	}

	public override string ToString()
	{
		return "CorridorID " + id + " d1(" + door1.x + "," + door1.y + ") d2(" + door2.x + "," + door2.y + ")";
	}

}
