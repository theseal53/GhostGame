using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingRoom : Landing
{
	StartingRoomTileSet tileset;

	public StartingRoom() : base()
	{
		roomCode = RoomCode.StartingRoom;
		widthRange = new IntRange(10, 14);
		heightRange = new IntRange(10, 14);
		tileset = (StartingRoomTileSet)TileSet;
	}

	public override void SetupRoom(Doorway doorway, int story)
	{
		base.SetupRoom(doorway, story);
		GenerateStartingStairways();
	}

	private void GenerateStartingStairways()
	{
		stairwayUp = new Vector2Int(Random.Range(x, x + width - stairwayBreadth), Random.Range(y, y + height - stairwayBreadth));
		stairwayDown = new Vector2Int(Random.Range(x, x + width - stairwayBreadth), Random.Range(y, y + height - stairwayBreadth));
	}

	public List<Vector2> StartingPositions()
	{
		return new List<Vector2>() {
			new Vector2(x + width / 2, y + height / 2),
			new Vector2(x + width / 2+1, y + height / 2),
			new Vector2(x + width / 2, y + height / 2+1),
			new Vector2(x + width / 2+1, y + height / 2+1)
		};

	}

	public override void GenerateFurniture()
	{
		GenerateLightSwitch();
		InstantiateFurniture(tileset.staircaseUp, stairwayUp);
		InstantiateFurniture(tileset.staircaseDown, stairwayDown);

		if (GameManager.I.isServer)
		{
			Vector2 randomSpot = new Vector2(Random.Range(x, x + width), Random.Range(y, y + width));
			Furniture obstacle = InstantiateFurniture(tileset.dresser, randomSpot);
			obstacle.transform.position = randomSpot;
			/*ItemContainer itemContainer = obstacle.GetComponent<ItemContainer>();
			Item item = Object.Instantiate(PrefabRegistry.I.crucifix).GetComponent<Item>();
			itemContainer.containedItem = item;
			item.gameObject.SetActive(false);*/
		}
	}
}
