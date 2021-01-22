using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingRoom : Room
{
	StartingRoomTileSet tileSet;

	public StartingRoom() : base()
	{
		roomCode = RoomCode.StartingRoom;
		widthRange = new IntRange(15, 20);
		heightRange = new IntRange(15, 20);
		tileSet = (StartingRoomTileSet)TileSet;
	}

	public override Doorway PossibleDoorway()
	{
		Direction desiredDirection = FindDesiredDoorDirection(new List<Direction>() { doorways[0].roomOutDirection });

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

	public List<Vector2> StartingPositions()
	{
		return new List<Vector2>() {
			new Vector2(x + width / 2, y + height / 2)
		};

	}

	public override void GenerateFurniture()
	{
		Vector2 randomSpot = new Vector2(Random.Range(x, x + width), Random.Range(y, y + width));
		GameObject obstacle = Object.Instantiate(tileSet.dresser);
		obstacle.transform.position = randomSpot;
		ItemContainer itemContainer = obstacle.GetComponent<ItemContainer>();
		Item item = Object.Instantiate(PrefabRegistry.I.crucifix).GetComponent<Item>();
		itemContainer.containedItem = item;
		item.gameObject.SetActive(false);
	}
}
