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
	public RoomCode roomCode = RoomCode.Test1;

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
			return TileSetRegistry.I.GetTileSet(roomCode);
		}
	}

	public virtual void SetupRoom(Board board, Doorway doorway)
	{
		SetRoomDimensions();

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

	public virtual void PrintToTilesArray(short[,] tiles)
	{
		for (int j = 0; j < width; j++)
		{
			int xCoord = x + j;

			for (int k = 0; k < height; k++)
			{
				int yCoord = y + k;
				tiles[yCoord, xCoord] = (short)roomCode;
			}
		}
	}

	delegate void MoveFunction(Room target);
	class MovePair : System.IComparable
	{
		public int distance;
		public MoveFunction action;
		public MovePair(int distance, MoveFunction action)
		{
			this.distance = distance;
			this.action = action;
		}

		public int CompareTo(object obj)
		{
			MovePair other = (MovePair)obj;
			if (distance > other.distance)
				return 1;
			if (distance < other.distance)
				return -1;
			return 0;
		}
	}

	public bool TestRoomValidity(Board board)
	{
		int margin = Constants.ROOM_MARGIN;

		bool movedNorth = false;
		bool movedSouth = false;
		bool movedEast = false;
		bool movedWest = false;

		if (x < board.boardMargin)
			x = board.boardMargin; movedEast = true;
		if (y < board.boardMargin)
			y = board.boardMargin; movedNorth = true;
		if (x + width > board.columns - board.boardMargin)
			x = board.columns - board.roomMargin - width; movedWest = true;
		if (y + height > board.rows - board.boardMargin)
			y = board.rows - board.boardMargin - height; movedSouth = true;

		bool moved = false;

		int northFreedom = 0;
		int southFreedom = 0;
		int eastFreedom = 0;
		int westFreedom = 0;

		GetDoorwayFreedoms(ref northFreedom, ref southFreedom, ref eastFreedom, ref westFreedom);

		MovePair moveEast = new MovePair(0, delegate (Room target) {
			if (!movedWest)
			{
				int desiredX = target.x + target.width + margin;
				int difference = desiredX - x;
				if (difference <= eastFreedom && desiredX + width <= board.columns - board.boardMargin)
				{
					eastFreedom -= difference;
					westFreedom += difference;
					movedEast = true;
					moved = true;
					x = desiredX;
				}
			}
		});
		MovePair moveWest = new MovePair(0, delegate (Room target) {
			if (!movedEast)
			{
				int desiredX = target.x - width - margin;
				int difference = x - desiredX;
				if (difference <= eastFreedom && desiredX >= board.boardMargin)
				{
					westFreedom -= difference;
					eastFreedom += difference;
					movedEast = true;
					moved = true;
					x = desiredX;
				}
			}
		});
		MovePair moveNorth = new MovePair(0, delegate (Room target) {
			if (!movedSouth)
			{
				int desiredY = target.y + target.height + margin;
				int difference = desiredY - y;
				if (difference <= northFreedom && desiredY + height <= board.rows - margin)
				{
					eastFreedom -= difference;
					westFreedom += difference;
					movedNorth = true;
					moved = true;
					y = desiredY;
				}
			}
		});
		MovePair moveSouth = new MovePair(0, delegate (Room target) {
			if (!movedNorth)
			{
				int desiredY = target.y - height - margin;
				int difference = y - desiredY;
				if (difference <= southFreedom && desiredY <= board.boardMargin)
				{
					westFreedom -= difference;
					eastFreedom += difference;
					movedSouth = true;
					moved = true;
					y = desiredY;
				}
			}
		});
		List<MovePair> options = new List<MovePair>() { moveEast, moveWest, moveNorth, moveSouth };

		List<Room> collisionRooms = new List<Room>(board.rooms);

		Rect rect = new Rect(x - margin, y - margin, width + margin, height + margin);

		for (int i = 0; i < collisionRooms.Count; i++)
		{
			Room room = collisionRooms[i];
			Rect otherRect = new Rect(room.x - room.margin, room.y - room.margin, room.width + room.margin, room.height + room.margin);
			if (rect.Overlaps(otherRect))
			{
				moved = false;

				moveEast.distance = x + width + margin - room.x;
				moveWest.distance = room.x + room.width + margin - x;
				moveNorth.distance = y + height + margin - room.y;
				moveSouth.distance = room.y + room.height + margin - y;

				options.Sort();
				foreach(MovePair movePair in options)
				{
					if (movePair.distance > 0)
					{
						movePair.action(room);
						if (moved)
							Debug.Log("Moved something!");
						if (moved)
							break;
					}
				}

				if (!moved)
				{
					return false;
				}
				//It won't be able to collide anymore
				collisionRooms.Remove(room);
				i = 0;
			}
		}
		return true;
	}

	private void GetDoorwayFreedoms(ref int northFreedom, ref int southFreedom, ref int eastFreedom, ref int westFreedom)
	{
		Doorway door = doorways[0];
		Corridor corridor = door.corridor;
		if (door.roomOutDirection == Direction.North)
		{
			northFreedom = corridor.length - corridor.lengthRange.m_Min;
			southFreedom = corridor.lengthRange.m_Max - corridor.length;
			eastFreedom = door.x - x;
			westFreedom = x + width - (door.x + door.breadth - 1);
		}
		if (door.roomOutDirection == Direction.South)
		{
			southFreedom = corridor.length - corridor.lengthRange.m_Min;
			northFreedom = corridor.lengthRange.m_Max - corridor.length;
			eastFreedom = door.x - x;
			westFreedom = x + width - (door.x + door.breadth - 1);
		}
		if (door.roomOutDirection == Direction.East)
		{
			eastFreedom = corridor.length - corridor.lengthRange.m_Min;
			westFreedom = corridor.lengthRange.m_Max - corridor.length;
			northFreedom = door.y - y;
			southFreedom = y+ height - (door.y + door.breadth - 1);
		}
		if (door.roomOutDirection == Direction.South)
		{
			westFreedom = corridor.length - corridor.lengthRange.m_Min;
			eastFreedom = corridor.lengthRange.m_Max - corridor.length;
			northFreedom = door.y - y;
			southFreedom = y + height - (door.y + door.breadth - 1);
		}
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
	protected GameObject AttemptObjectInstantiation(GameObject prefab, Rect rect)
	{
		if (!ObstructsDoorway(rect))
		{
			return Object.Instantiate(prefab, new Vector3(rect.x, rect.y), Quaternion.identity);
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
	public virtual void PostValiditySetup()
	{
		//Do nothing
	}

	public override string ToString()
	{
		return "RoomID " + roomCode + " x,y(" + x + "," + y + ") dims(" + width + "," + height + ")";
	}
}
