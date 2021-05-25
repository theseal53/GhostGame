using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cellar : Room
{
	private CellarTileset tileset;

	private Orientation orientation;

	IntRange shortRange = new IntRange(5, 6);
	IntRange longRange = new IntRange(13, 16);

	List<Furniture> furnitureOptions;
	List<float> furnitureChances;

	public Cellar() : base()
	{
		roomCode = RoomCode.Cellar;
		tileset = (CellarTileset)TileSetRegistry.I.GetTileSet(roomCode);

		furnitureOptions = new List<Furniture>()
		{
			tileset.largeWineShelf,
			tileset.smallWineShelf
		};
		furnitureChances = new List<float>()
		{
			.6f,
			.4f
		};
	}

	protected override void SetRoomDimensions()
	{
		orientation = DirectionUtil.Orientation(doorways[0].roomOutDirection);
		if (orientation == Orientation.Horizontal)
		{
			widthRange = longRange;
			heightRange = shortRange;
		}
		else
		{
			widthRange = shortRange;
			heightRange = longRange;
		}
		width = widthRange.Random;
		height = heightRange.Random;
	}

	public override void GenerateFurniture()
	{
		GenerateLightSwitch();
		float precompTotal = WeightedChoice.PrecompTotal(furnitureChances);
		if (orientation == Orientation.Horizontal)
		{
			for (int xPos = x + 1; xPos < x + width - 1;)
			{
				Furniture prefab;
				if (xPos < x + width - 2)
					prefab = RandomFurniture(precompTotal);
				else
					prefab = tileset.smallWineShelf;
				int furnitureBreadth;
				if (prefab == tileset.largeWineShelf)
					furnitureBreadth = 2;
				else
					furnitureBreadth = 1;

				InstantiateFurniture(prefab, new Vector2(xPos, y));
				xPos += furnitureBreadth;
			}
			for (int xPos = x + 1; xPos < x + width - 1;)
			{
				Furniture prefab;
				if (xPos < x + width - 2)
					prefab = RandomFurniture(precompTotal);
				else
					prefab = tileset.smallWineShelf;
				int furnitureBreadth;
				if (prefab == tileset.largeWineShelf)
					furnitureBreadth = 2;
				else
					furnitureBreadth = 1;

				InstantiateFurniture(prefab, new Vector2(xPos, y + height - furnitureBreadth));
				xPos += furnitureBreadth;
			}
		}
		else  //Orientation.Vertical
		{
			for (int yPos = y + 1; yPos < y + height - 1;)
			{
				Furniture prefab;
				if (yPos < y + height - 2)
					prefab = RandomFurniture(precompTotal);
				else
					prefab = tileset.smallWineShelf;
				int furnitureBreadth;
				if (prefab == tileset.largeWineShelf)
					furnitureBreadth = 2;
				else
					furnitureBreadth = 1;

				InstantiateFurniture(prefab, new Vector2(x, yPos));
				yPos += furnitureBreadth;
			}
			for (int yPos = y + 1; yPos < y + height - 1;)
			{
				Furniture prefab;
				if (yPos < y + height - 2)
					prefab = RandomFurniture(precompTotal);
				else
					prefab = tileset.smallWineShelf;
				int furnitureBreadth;
				if (prefab == tileset.largeWineShelf)
					furnitureBreadth = 2;
				else
					furnitureBreadth = 1;

				InstantiateFurniture(prefab, new Vector2(x + width - furnitureBreadth, yPos));
				yPos += furnitureBreadth;
			}
		}
	}


	private Furniture RandomFurniture(float precompTotal)
	{
		return WeightedChoice.Choose(furnitureOptions, furnitureChances, precompTotal);
	}

	public override bool CanMakeMoreDoors()
	{
		if (doorways.Count == 2)
			return false;
		return true;
	}

	public override Doorway PossibleDoorway()
	{
		Direction desiredDirection = DirectionUtil.Reverse(doorways[0].roomOutDirection);

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


}
