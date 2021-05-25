using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : Room
{
	KitchenTileset tileSet;

	int pantryBreadth;
	int pantryLength;
	IntRange pantryBreadthRange = new IntRange(3, 4);
	IntRange pantryLengthRange = new IntRange(5, 8);
	Direction pantryDirection;
	private Vector2Int kitchenDoorPosition;
	private Vector2Int kitchenPantryMedium;
	private Vector2Int pantryDoorPosition;

	int pantryWall = 1;

	int kX, kY, kWidth, kHeight;
	int pX, pY, pWidth, pHeight;

	List<Furniture> pantryOptions;
	List<float> pantryChances;
	List<Furniture> pantryCornerOptions;
	List<float> pantryCornerChances;

	int counterMargin = 3;
	int counterMinBreadth = 2;

	public Kitchen() : base()
	{
		roomCode = RoomCode.Kitchen;
		widthRange = new IntRange(12, 20);
		heightRange = new IntRange(12, 20);
		tileSet = (KitchenTileset)TileSet;

		pantryOptions = new List<Furniture>()
		{
			tileSet.crate,
			tileSet.potatoSack,
			tileSet.flourSack,
			null
		};
		pantryChances = new List<float>()
		{
			.6f,
			.1f,
			.1f,
			.2f
		};
		pantryCornerOptions = new List<Furniture>()
		{
			tileSet.potatoSack,
			tileSet.flourSack,
			null
		};
		pantryCornerChances = new List<float>()
		{
			.3f,
			.3f,
			.4f
		};
	}

	public override void SetupRoom(Doorway doorway, int story)
	{
		base.SetupRoom(doorway, story);
		SetKitchenPantryDimensions();
	}

	public override void PostValiditySetup()
	{
		SetKitchenPantryDimensions();
	}

	public override void GenerateFurniture()
	{
		GenerateLightSwitch();
		GeneratePantryFurniture();
		GenerateKitchenFurniture();
	}

	private void SetKitchenPantryDimensions()
	{
		pantryBreadth = pantryBreadthRange.Random;
		pantryLength = pantryLengthRange.Random;
		//Need to finish this

		Orientation orientation;
		if (width > height)
			orientation = Orientation.Horizontal;
		else
			orientation = Orientation.Vertical;

		if (DirectionUtil.Orientation(doorways[0].roomOutDirection) == orientation)
		{
			pantryDirection = DirectionUtil.Reverse(doorways[0].roomOutDirection);
		}
		else
		{
			if (DirectionUtil.Orientation(doorways[0].roomOutDirection) == Orientation.Vertical)
			{
				if (pX + pantryBreadth + pantryWall >= doorways[0].x)
					pantryDirection = Direction.East;
				else if (x + width - pantryBreadth - pantryWall < doorways[0].x)
					pantryDirection = Direction.West;
				else
					pantryDirection = Random.value > .5f ? Direction.East : Direction.West;
			}
			else
			{
				if (pY + pantryBreadth + pantryWall >= doorways[0].y)
					pantryDirection = Direction.North;
				else if (y + height - pantryBreadth - pantryWall < doorways[0].y)
					pantryDirection = Direction.South;
				else
					pantryDirection = Random.value > .5f ? Direction.North : Direction.South;
			}
		}

		if (pantryDirection == Direction.North)
		{
			kX = x; kY = y; kWidth = width; kHeight = height - pantryBreadth - pantryWall;
			pX = x; pY = y + height - pantryBreadth; pWidth = width; pHeight = pantryBreadth;
			pantryDoorPosition = new Vector2Int(Random.Range(pX + 1, pX + pWidth - 2), pY);
			kitchenPantryMedium = new Vector2Int(pantryDoorPosition.x, pantryDoorPosition.y - 1);
			kitchenDoorPosition = new Vector2Int(pantryDoorPosition.x, pantryDoorPosition.y - 2);
		}
		else if (pantryDirection == Direction.South)
		{
			kX = x; kY = y + pantryBreadth + pantryWall; kWidth = width; kHeight = height - pantryBreadth - pantryWall;
			pX = x; pY = y; pWidth = width; pHeight = pantryBreadth;
			pantryDoorPosition = new Vector2Int(Random.Range(pX + 1, pX + pWidth - 2), pY + pHeight - 1);
			kitchenPantryMedium = new Vector2Int(pantryDoorPosition.x, pantryDoorPosition.y + 1);
			kitchenDoorPosition = new Vector2Int(pantryDoorPosition.x, pantryDoorPosition.y + 2);
		}
		else if (pantryDirection == Direction.East)
		{
			kX = x; kY = y; kWidth = width - pantryBreadth - pantryWall; kHeight = height;
			pX = x + width - pantryBreadth; pY = y; pWidth = pantryBreadth; pHeight = height;
			pantryDoorPosition = new Vector2Int(pX, Random.Range(pY + 1, pY + pHeight - 2));
			kitchenPantryMedium = new Vector2Int(pantryDoorPosition.x - 1, pantryDoorPosition.y);
			kitchenDoorPosition = new Vector2Int(pantryDoorPosition.x - 2, pantryDoorPosition.y);
		}
		else
		{
			kX = x + pantryBreadth + pantryWall; kY = y; kWidth = width - pantryBreadth - pantryWall; kHeight = height;
			pX = x; pY = y; pWidth = pantryBreadth; pHeight = height;
			pantryDoorPosition = new Vector2Int(pX + pWidth - 1, Random.Range(pY + 1, pY + pHeight - 2));
			kitchenPantryMedium = new Vector2Int(pantryDoorPosition.x + 1, pantryDoorPosition.y);
			kitchenDoorPosition = new Vector2Int(pantryDoorPosition.x + 2, pantryDoorPosition.y);
		}
	}

	public void GeneratePantryFurniture()
	{
		//Corners
		float precompTotal = WeightedChoice.PrecompTotal(pantryCornerChances);
		Furniture prefab = RandomPantryItem(precompTotal);
		if (prefab != null)
		{
			InstantiateFurniture(prefab, new Vector2(pX, pY));
		}
		prefab = RandomPantryItem(precompTotal);
		if (prefab != null)
		{
			InstantiateFurniture(prefab, new Vector2(pX + pWidth - 1, pY));
		}
		prefab = RandomPantryItem(precompTotal);
		if (prefab != null)
		{
			InstantiateFurniture(prefab, new Vector2(pX, pY + pHeight - 1));
		}
		prefab = RandomPantryItem(precompTotal);
		if (prefab != null)
		{
			InstantiateFurniture(prefab, new Vector2(pX + pWidth - 1, pY + pHeight - 1));
		}

		//Sides
		precompTotal = WeightedChoice.PrecompTotal(pantryChances);
		//Horizontal sides
		for (int xPos = pX + 1; xPos < pX + pWidth - 1; xPos++)
		{
			prefab = RandomPantryItem(precompTotal);
			Vector2 position = new Vector3(xPos, pY);
			if (prefab != null && position != pantryDoorPosition)
			{
				InstantiateFurniture(prefab, position);
			}
			prefab = RandomPantryItem(precompTotal);
			position = new Vector3(xPos, pY + pHeight - 1);
			if (prefab != null && position != pantryDoorPosition)
			{
				InstantiateFurniture(prefab, position);
			}
		}
		//Vertical sides
		for (int yPos = pY + 1; yPos < pY + pHeight - 1; yPos++)
		{
			prefab = RandomPantryItem(precompTotal);
			Vector2 position = new Vector3(pX, yPos);
			if (prefab != null && position != pantryDoorPosition)
			{
				InstantiateFurniture(prefab, position);
			}
			prefab = RandomPantryItem(precompTotal);
			position = new Vector3(pX + pWidth - 1, yPos);
			if (prefab != null && position != pantryDoorPosition)
			{
				InstantiateFurniture(prefab, position);
			}
		}
	}

	public void GenerateKitchenFurniture()
	{
		GenerateCenterCounter();
		//Generate the corners
		float rng = Random.value;
		Furniture firstSpawn = rng > .5f ? tileSet.fridge : tileSet.oven;
		Furniture secondSpawn = rng > .5f ? tileSet.oven : tileSet.fridge;
		Rect firstSpawnRect;
		Rect secondSpawnRect;
		Rect firstCornerFiller;
		Rect secondCornerFiller;
		if (doorways[0].roomOutDirection == Direction.North)
		{
			firstSpawnRect = new Rect(kX, kY, 2, 2);
			secondSpawnRect = new Rect(kX + kWidth - 2, kY, 2, 2);
			firstCornerFiller = new Rect(kX, kY + kHeight - 1, 1, 1);
			secondCornerFiller = new Rect(kX + kWidth - 1, kY + kHeight - 1, 1, 1);
		}
		else if (doorways[0].roomOutDirection == Direction.South)
		{
			firstSpawnRect = new Rect(kX, kY + kHeight - 2, 2, 2);
			secondSpawnRect = new Rect(kX + kWidth - 2, kY + kHeight - 2, 2, 2);
			firstCornerFiller = new Rect(kX, kY, 1, 1);
			secondCornerFiller = new Rect(kX + kWidth - 1, kY, 1, 1);
		}
		else if (doorways[0].roomOutDirection == Direction.East)
		{
			firstSpawnRect = new Rect(kX, kY, 2, 2);
			secondSpawnRect = new Rect(kX, kY + kHeight - 2, 2, 2);
			firstCornerFiller = new Rect(kX + kWidth - 1, kY, 1, 1);
			secondCornerFiller = new Rect(kX + kWidth - 1, kY + kHeight - 1, 1, 1);
		}
		else //(doorways[0].roomOutDirection == Direction.West)
		{
			firstSpawnRect = new Rect(kX + kWidth - 2, kY + kHeight - 2, 2, 2);
			secondSpawnRect = new Rect(kX + kWidth - 2, kY, 2, 2);
			firstCornerFiller = new Rect(kX, kY, 1, 1);
			secondCornerFiller = new Rect(kX, kY + kHeight - 1, 1, 1);
		}
		AttemptFurnitureInstantiation(firstSpawn, firstSpawnRect);
		AttemptFurnitureInstantiation(secondSpawn, secondSpawnRect);
		AttemptFurnitureInstantiation(tileSet.plainCounter, firstCornerFiller);
		AttemptFurnitureInstantiation(tileSet.plainCounter, secondCornerFiller);

		List<Vector2> availableSpots = new List<Vector2>();
		for (int xPos = kX + 1; xPos < kX + kWidth - 1; xPos++)
		{
			Rect rect = new Rect(xPos, kY, 1, 1);
			if (!ObstructsDoorway(rect) && !firstSpawnRect.Overlaps(rect) && !secondSpawnRect.Overlaps(rect) && rect.position != kitchenDoorPosition)
				availableSpots.Add(new Vector2(rect.x, rect.y));
			rect.y = kY + kHeight - 1;
			if (!ObstructsDoorway(rect) && !firstSpawnRect.Overlaps(rect) && !secondSpawnRect.Overlaps(rect) && rect.position != kitchenDoorPosition)
				availableSpots.Add(new Vector2(rect.x, rect.y));
		}
		for (int yPos = kY + 1; yPos < kY + kHeight - 1; yPos++)
		{
			Rect rect = new Rect(kX, yPos, 1, 1);
			if (!ObstructsDoorway(rect) && !firstSpawnRect.Overlaps(rect) && !secondSpawnRect.Overlaps(rect) && rect.position != kitchenDoorPosition)
				availableSpots.Add(new Vector2(rect.x, rect.y));
			rect.x = kX + kWidth - 1;
			if (!ObstructsDoorway(rect) && !firstSpawnRect.Overlaps(rect) && !secondSpawnRect.Overlaps(rect) && rect.position != kitchenDoorPosition)
				availableSpots.Add(new Vector2(rect.x, rect.y));
		}


		foreach(Vector2 spot in availableSpots)
		{
			InstantiateFurniture(tileSet.cabinetCounter, spot);
		}

	}

	public void GenerateCenterCounter()
	{
		//Find the dimensions
		int cX = Random.Range(kX + counterMargin, kX + kWidth - counterMinBreadth - counterMargin);
		int cY = Random.Range(kY + counterMargin, kY + kHeight - counterMinBreadth - counterMargin);
		int cWidth = Random.Range(counterMinBreadth, kX + kWidth - counterMargin - cX);
		int cHeight = Random.Range(counterMinBreadth, kY + kHeight - counterMargin - cY);

		//Corners
		TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(cX, cY, 0), tileSet.counterBottomLeft);
		TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(cX + cWidth - 1, cY, 0), tileSet.counterBottomRight);
		TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(cX, cY + cHeight - 1, 0), tileSet.counterTopLeft);
		TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(cX + cWidth - 1, cY + cHeight - 1, 0), tileSet.counterTopRight);

		for (int xPos = cX + 1; xPos < cX + cWidth - 1; xPos++)
		{
			TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(xPos, cY, 0), tileSet.counterBottomCenter);
			TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(xPos, cY + cHeight - 1, 0), tileSet.counterTopCenter);
		}
		for (int yPos = cY + 1; yPos < cY + cHeight - 1; yPos++)
		{
			TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(cX, yPos, 0), tileSet.counterMidLeft);
			TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(cX + cWidth - 1, yPos, 0), tileSet.counterMidRight);
		}
		for (int xPos = cX + 1; xPos < cX + cWidth - 1; xPos++)
		{
			for (int yPos = cY + 1; yPos < cY + cHeight - 1; yPos++)
			{
				TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(xPos, yPos, 0), tileSet.counterMidCenter);
			}
		}

		Vector2 position = new Vector2(cX + cWidth / 2f - .5f, cY + cHeight / 2f - .5f);
		Furniture counterCollider = InstantiateFurniture(PrefabRegistry.I.boxOverlay.GetComponent<Furniture>(), position);
		counterCollider.transform.localScale = new Vector2(cWidth, cHeight);
	}

	public Furniture RandomPantryCornerItem(float precompTotal)
	{
		return WeightedChoice.Choose(pantryCornerOptions, pantryCornerChances, precompTotal);
	}

	public Furniture RandomPantryItem(float precompTotal)
	{
		return WeightedChoice.Choose(pantryOptions, pantryChances, precompTotal);
	}

	public override Doorway PossibleDoorway()
	{
		Direction desiredDirection = FindDesiredDoorDirection(new List<Direction>() { pantryDirection });

		int northFridgeDiscount = doorways[0].roomOutDirection == Direction.North ? 0 : 2;
		int southFridgeDiscount = doorways[0].roomOutDirection == Direction.South ? 0 : 2;
		int eastFridgeDiscount = doorways[0].roomOutDirection == Direction.East ? 0 : 2;
		int westFridgeDiscount = doorways[0].roomOutDirection == Direction.West ? 0 : 2;

		switch (desiredDirection)
		{
			case Direction.North:
				return new Doorway(Random.Range(kX + westFridgeDiscount, kX + kWidth - generatedDoorwayBreadth - eastFridgeDiscount), kY + kHeight - 1, Direction.North);
			case Direction.South:
				return new Doorway(Random.Range(kX + westFridgeDiscount, kX + kWidth - generatedDoorwayBreadth - eastFridgeDiscount), kY, Direction.South);
			case Direction.East:
				return new Doorway(kX + kWidth - 1, Random.Range(kY + southFridgeDiscount, kY + kHeight - generatedDoorwayBreadth - northFridgeDiscount), Direction.East);
			default:
				return new Doorway(kX, Random.Range(kY + southFridgeDiscount, kY + kHeight - generatedDoorwayBreadth - northFridgeDiscount), Direction.West);
		}
	}

	public override void PrintToTilesArray(sbyte[][][] tiles)
	{
		for (int yPos = kY; yPos < kY + kHeight; yPos++)
		{
			for (int xPos = kX; xPos < kX + kWidth; xPos++)
			{
				tiles[story][yPos][xPos] = (sbyte)roomCode;
			}
		}
		for (int yPos = pY; yPos < pY + pHeight; yPos++)
		{
			for (int xPos = pX; xPos < pX + pWidth; xPos++)
			{
				tiles[story][yPos][xPos] = (sbyte)roomCode;
			}
		}
		tiles[story][kitchenPantryMedium.y][kitchenPantryMedium.x] = (sbyte)roomCode;
	}
}
