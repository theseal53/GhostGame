//Taken largely from https://www.youtube.com/watch?v=wnoLaui3uO4

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public int x;
	public int y;
	public int width;
	public int height;
	public int id;

	public List<Room> adjacentRooms = new List<Room>();
	public List<Doorway> doorways = new List<Doorway>();

	protected IntRange widthRange = new IntRange(6, 12);
	protected IntRange heightRange = new IntRange(6, 12);

	protected int generatedDoorwayBreadth = Constants.DEFAULT_DOOR_BREADTH;
	protected int margin = 1;

	public virtual TileSet TileSet
	{
		get
		{
			return TileSetRegistry.I.Test1;
		}
	}

	public virtual void SetupRoom(Board board, Doorway doorway, int roomId)
	{
		SetRoomDimensions();
		id = roomId;

		switch (doorway.roomOutDirection)
		{
			case Direction.South:
				//height = Mathf.Clamp(height, 1, board.rows - doorway.y);
				y = doorway.y;
				//doorway.x+1, to include doorway.x in the exclusive range
				x = Random.Range(doorway.x - width + doorway.breadth, doorway.x + 1);
				//x = Mathf.Clamp(x, 0, board.columns - width);
				break;
			case Direction.West:
				//width = Mathf.Clamp(width, 1, board.columns - doorway.x + 1);
				x = doorway.x;
				y = Random.Range(doorway.y - height + doorway.breadth, doorway.y + 1);
				//y = Mathf.Clamp(y, 0, board.rows - height);
				break;
			case Direction.North:
				//height = Mathf.Clamp(height, 1, doorway.y);
				y = doorway.y - height + 1;
				x = Random.Range(doorway.x - width + doorway.breadth, doorway.x + 1);
				//x = Mathf.Clamp(x, 0, board.columns - width);
				break;
			case Direction.East:
				//width = Mathf.Clamp(width, 1, doorway.x);
				x = doorway.x - width + 1;
				y = Random.Range(doorway.y - height + doorway.breadth, doorway.y + 1);
				//y = Mathf.Clamp(y, 0, board.rows - height);
				break;
		}
		doorways.Add(doorway);
	}

	protected virtual void SetRoomDimensions()
	{
		width = widthRange.Random;
		height = heightRange.Random;
	}

	public virtual Doorway PossibleDoorway()
	{
		Direction desiredDirection = FindDesiredDoorDirection();

		switch (desiredDirection)
		{
			case Direction.North:
				return new Doorway(Random.Range(x, x + width - generatedDoorwayBreadth), y + height - 1, Direction.North);
			case Direction.South:
				return new Doorway(Random.Range(x, x + width - generatedDoorwayBreadth), y, Direction.South);
			case Direction.East:
				return new Doorway(x + width - 1, Random.Range(y, y + height - generatedDoorwayBreadth), Direction.East);
			default:
				return new Doorway(x, Random.Range(y, y + height - generatedDoorwayBreadth), Direction.West);
		}
	}

	protected Direction FindDesiredDoorDirection(List<Direction> exclusions = null)
	{
		const int NUM_DIRECTIONS = 4;
		int[] counts = new int[NUM_DIRECTIONS];
		foreach (Doorway door in doorways)
		{
			counts[(int)door.roomOutDirection]++;
		}

		List<int> smallest = new List<int>();
		int smallestCount = -1;

		for (int i = 0; i < NUM_DIRECTIONS; i++)
		{
			if (exclusions == null || !exclusions.Contains((Direction)i))
			{
				if (smallest.Count == -1 || counts[i] <= smallestCount)
				{
					if (counts[i] < smallestCount)
					{
						smallest.Clear();
						smallestCount = counts[i];
					}
				}
				smallest.Add(i);
			}
		}

		return (Direction)smallest[Random.Range(0, smallest.Count)];
	}

	public void PrintToTilesArray(int[][] tiles)
	{
		for (int j = 0; j < width; j++)
		{
			int xCoord = x + j;

			for (int k = 0; k < height; k++)
			{
				int yCoord = y + k;
				tiles[yCoord][xCoord] = id;
			}
		}
	}

	public bool TestRoomValidity(Board board)
	{
		bool fitsRoomSpecs = CheckRoomSpecs();
		if (!fitsRoomSpecs)
			return false;

		bool outOfBounds = CheckOutOfBounds(board);
		if (outOfBounds)
			return false;

		bool intersecting = IntersectingOtherRooms(board);
		if (intersecting)
			return false;

		return true;
	}

	private bool CheckRoomSpecs()
	{
		if (width >= widthRange.m_Min &&
			width <= widthRange.m_Max &&
			height >= heightRange.m_Min &&
			height <= heightRange.m_Max)
			return true;
		return false;
	}

	private bool CheckOutOfBounds(Board board)
	{
		if (x < board.roomMargin ||
			y < board.roomMargin ||
			x + width > board.columns - board.roomMargin ||
			y + height > board.rows - board.roomMargin)
		{
			return true;
		}
		return false;
	}

	private bool IntersectingOtherRooms(Board board)
	{
		Rect rect = new Rect(x - margin, y - margin, width + margin, height + margin);
		foreach (Room room in board.rooms)
		{
			Rect otherRect = new Rect(room.x - room.margin, room.y - room.margin, room.width + room.margin, room.height + room.margin);
			if (rect.Overlaps(otherRect))
			{
				return true;
			}
		}
		return false;
	}

	public virtual void GenerateFurniture()
	{
		//Do nothing
	}

	public virtual void GenerateLights()
	{
		GameObject light = Object.Instantiate(PrefabRegistry.I.light);
		light.transform.position = new Vector2(x + width / 2, y + width / 2);
	}

	protected GameObject AttemptObjectInstantiation(GameObject prefab, int x, int y, int width, int height)
	{
		if (!ObstructsDoorway(x, y, width, height))
		{
			return Object.Instantiate(prefab, new Vector3(x, y), Quaternion.identity);
		}
		else
		{
			return null;
		}
	}

	protected bool ObstructsDoorway(int x, int y, int width, int height)
	{
		Rect rect = new Rect(x, y, width, height);
		return ObstructsDoorway(rect);
	}

	protected bool ObstructsDoorway(Rect rect)
	{
		foreach (Doorway doorway in doorways)
		{
			Rect doorRect;
			if (doorway.roomOutDirection == Direction.North | doorway.roomOutDirection == Direction.South)
			{
				doorRect = new Rect(doorway.x, doorway.y, doorway.breadth, 1);
			}
			else
			{
				doorRect = new Rect(doorway.x, doorway.y, 1, doorway.breadth);
			}
			if (rect.Overlaps(doorRect))
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool CanMakeMoreDoors()
	{
		return true;
	}

	public override string ToString()
	{
		return "RoomID " + id + " x,y(" + x + "," + y + ") dims(" + width + "," + height + ")";
	}
}
