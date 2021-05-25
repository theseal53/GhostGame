//Taken largely from https://www.youtube.com/watch?v=wnoLaui3uO4

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public int x;
	public int y;
	public int story;
	public int width;
	public int height;
	public RoomCode roomCode = RoomCode.Test1;

	public List<Room> adjacentRooms = new List<Room>();
	public List<Doorway> doorways = new List<Doorway>();

	protected IntRange widthRange = new IntRange(6, 12);
	protected IntRange heightRange = new IntRange(6, 12);

	protected int generatedDoorwayBreadth = Constants.DEFAULT_DOOR_BREADTH;
	protected int margin = 1;

	public LightSwitch lightSwitch;

	public virtual TileSet TileSet
	{
		get
		{
			return TileSetRegistry.I.GetTileSet(roomCode);
		}
	}

	public virtual void SetupRoom(Doorway doorway, int story)
	{
		this.story = story;
		doorways.Add(doorway);
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

	public virtual void PrintToTilesArray(sbyte[][][] tiles)
	{
		for (int j = 0; j < width; j++)
		{
			int xCoord = x + j;

			for (int k = 0; k < height; k++)
			{
				int yCoord = y + k;
				tiles[story][yCoord][xCoord] = (sbyte)roomCode;
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

	public virtual bool TestRoomValidity(Board board)
	{
		int margin = Constants.ROOM_MARGIN;

		bool movedNorth = false;
		bool movedSouth = false;
		bool movedEast = false;
		bool movedWest = false;

		int cachedX = x;
		int cachedY = y;

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

		List<Room> collisionRooms = new List<Room>(board.stories[story].rooms);

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
		//If it all worked, make corrections to doorways and hallways
		foreach(Doorway doorway in doorways)
		{
			if (DirectionUtil.Orientation(doorway.roomOutDirection) == Orientation.Horizontal)
			{
				doorway.x += x - cachedX;
				doorway.corridor.length += x - cachedX;
			}
			else
			{
				doorway.y += y - cachedY;
				doorway.corridor.length += y - cachedY;
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
		GenerateLightSwitch();
	}

	public virtual void GenerateLightSwitch()
	{
		Doorway target = doorways[0];
		Vector2 position = new Vector2(target.x, target.y);
		if (target.roomOutDirection == Direction.North)
		{
			position.y += 1;
			if (Random.value > .5f || target.x == x)
				position.x += target.breadth;
			else
				position.x -= 1;
		}
		else if (target.roomOutDirection == Direction.South)
		{
			if (Random.value > .5f || target.x == x)
				position.x += target.breadth;
			else
				position.x -= 1;
		}
		else if (target.roomOutDirection == Direction.East)
		{
			if (Random.value > .5f || target.y == y)
				position.y += target.breadth;
			else
				position.y -= 1;
		}
		else if (target.roomOutDirection == Direction.West)
		{
			if (Random.value > .5f || target.y == y)
				position.y += target.breadth;
			else
				position.y -= 1;
		}
		lightSwitch = (LightSwitch)InstantiateFurniture(PrefabRegistry.I.lightSwitch.GetComponent<LightSwitch>(), position);
		if (lightSwitch)
		{
			lightSwitch.GetComponent<DoubleCardinalSprite>().UpdateDirection(DirectionUtil.Reverse(target.roomOutDirection));
		}
		lightSwitch.gameObject.name = GetType().Name + " LightSwitch";
	}

	public virtual void GenerateLights()
	{
		GenerateDoorwayLights();
		GenerateInnerRoomLights();
	}

	public virtual void GenerateDoorwayLights()
	{
		foreach (Doorway doorway in doorways)
		{
			Vector2 position = new Vector2(doorway.x, doorway.y);
			if (doorway.roomOutDirection == Direction.North)
			{
				position.y += 1;
				if (Random.value > .5f || doorway.x == x)
					position.x += doorway.breadth;
				else
					position.x -= 1;
			}
			else if (doorway.roomOutDirection == Direction.South)
			{
				if (Random.value > .5f || doorway.x == x)
					position.x += doorway.breadth;
				else
					position.x -= 1;
			}
			else if (doorway.roomOutDirection == Direction.East)
			{
				if (Random.value > .5f || doorway.y == y)
					position.y += doorway.breadth;
				else
					position.y -= 1;
			}
			else if (doorway.roomOutDirection == Direction.West)
			{
				if (Random.value > .5f || doorway.y == y)
					position.y += doorway.breadth;
				else
					position.y -= 1;
			}
			ElectricLamp electricLamp = (ElectricLamp)InstantiateFurniture(PrefabRegistry.I.standardWallLight.GetComponent<ElectricLamp>(), position);
			electricLamp.GetComponent<CardinalSprite>().UpdateDirection(DirectionUtil.Reverse(doorway.roomOutDirection));
			lightSwitch.AddChild(electricLamp);
		}
	}

	public virtual void GenerateInnerRoomLights()
	{
		Vector2 position = new Vector2(x + width / 2, y + height / 2);
		ElectricLamp electricLamp = (ElectricLamp)InstantiateFurniture(PrefabRegistry.I.ceilingChainLight.GetComponent<Furniture>(), position);
		lightSwitch.AddChild(electricLamp);
	}

	protected Furniture InstantiateFurniture(Furniture prefab, Vector2 position)
	{
		if (!GameManager.I.isServer && prefab.spawnOnServer) { return null; }
		Furniture newFurniture = Object.Instantiate(prefab, position, Quaternion.identity);
		newFurniture.Init(story);
		if (newFurniture != null && GameManager.I.isServer && newFurniture.spawnOnServer)
		{
			NetworkServer.Spawn(newFurniture.gameObject);
		}
		return newFurniture;
	}

	protected Furniture AttemptFurnitureInstantiation(Furniture prefab, int x, int y, int width, int height)
	{
		if (!GameManager.I.isServer && prefab.spawnOnServer) { return null; }
		if (!ObstructsDoorway(x, y, width, height) && !ObstructsLightSwitch(x, y, width, height))
		{
			Furniture newFurniture = Object.Instantiate(prefab, new Vector3(x, y), Quaternion.identity);
			newFurniture.Init(story);
			if (newFurniture != null && GameManager.I.isServer && newFurniture.spawnOnServer)
			{
				NetworkServer.Spawn(newFurniture.gameObject);
			}
			return newFurniture;
		}
		return null;
	}
	protected Furniture AttemptFurnitureInstantiation(Furniture prefab, Rect rect)
	{
		if (!GameManager.I.isServer && prefab.spawnOnServer) { return null; }
		if (!ObstructsDoorway(rect) && !ObstructsLightSwitch(rect))
		{
			Furniture newFurniture = Object.Instantiate(prefab, new Vector3(rect.x, rect.y), Quaternion.identity);
			newFurniture.Init(story);
			if (newFurniture != null && GameManager.I.isServer && newFurniture.spawnOnServer)
			{
				NetworkServer.Spawn(newFurniture.gameObject);
			}
			return newFurniture;
		}
		return null;
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

	protected bool ObstructsLightSwitch(int x, int y, int width, int height)
	{
		Rect rect = new Rect(x, y, width, height);
		return ObstructsLightSwitch(rect);
	}
	protected bool ObstructsLightSwitch(Rect rect)
	{
		Rect switchRect = new Rect(lightSwitch.transform.position, Vector2.one);
		if (rect.Overlaps(switchRect))
		{
			return true;
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
