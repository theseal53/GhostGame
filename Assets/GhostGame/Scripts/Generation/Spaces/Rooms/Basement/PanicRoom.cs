using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicRoom : SmallBedroom
{
	private PanicRoomTileset tileSet;

	List<Furniture> borderOptions;
	List<float> borderChances;

	private int doorwayBreadth = 1;

	public PanicRoom() : base()
	{
		roomCode = RoomCode.PanicRoom;
		widthRange = new IntRange(5, 8);
		heightRange = new IntRange(5, 8);
		tileSet = (PanicRoomTileset)TileSetRegistry.I.GetTileSet(roomCode);

		borderOptions = new List<Furniture>()
		{
			tileSet.crate,
			tileSet.potatoSack,
			tileSet.flourSack,
			null
		};
		borderChances = new List<float>()
		{
			.6f,
			.1f,
			.1f,
			.2f
		};
	}

	public override void GenerateFurniture()
	{
		GenerateLightSwitch();
		/*Rect bedRect = GenerateCornerBed(tileSet.horizontalBed, tileSet.verticalBed);

		List<Vector2> availableSpots = new List<Vector2>();
		for (int xPos = x + 1; xPos < x + width - 1; xPos++)
		{
			Rect rect = new Rect(xPos, y, 1, 1);
			if (!ObstructsDoorway(rect) && !bedRect.Overlaps(rect))
				availableSpots.Add(new Vector2(rect.x, rect.y));
			rect.y = y + height - 1;
			if (!ObstructsDoorway(rect) && !bedRect.Overlaps(rect))
				availableSpots.Add(new Vector2(rect.x, rect.y));
		}
		for (int yPos = y + 1; yPos < y + height - 1; yPos++)
		{
			Rect rect = new Rect(x, yPos, 1, 1);
			if (!ObstructsDoorway(rect) && !bedRect.Overlaps(rect))
				availableSpots.Add(new Vector2(rect.x, rect.y));
			rect.x = x + width - 1;
			if (!ObstructsDoorway(rect) && !bedRect.Overlaps(rect))
				availableSpots.Add(new Vector2(rect.x, rect.y));
		}

		float precompTotal = WeightedChoice.PrecompTotal(borderChances);

		foreach (Vector2 spot in availableSpots)
		{
			Furniture prefab = RandomBorderFurniture(precompTotal);
			if (prefab != null)
				InstantiateFurniture(prefab, spot);
		}*/
	}

	private Furniture RandomBorderFurniture(float precompTotal)
	{
		return WeightedChoice.Choose(borderOptions, borderChances, precompTotal);
	}

	

	public override bool CanMakeMoreDoors()
	{
		return false;
	}

	public override void PostValiditySetup()
	{
		doorways[0].corridor.UpdateBreadth(doorwayBreadth);
	}
}
