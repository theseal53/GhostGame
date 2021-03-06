﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiningRoom : Room
{
	IntRange diningTableBreadthRange = new IntRange(3, 5);
	IntRange diningTableLengthRange = new IntRange(6, 10);
	IntRange diningTableMarginRange = new IntRange(3, 4);

	int diningTableBreadth;
	int diningTableLength;
	int diningTableMargin;
	float chairGenerationChance = .5f;

	bool vertical;

	DiningRoomTileset tileSet;

	public DiningRoom() : base()
	{
		roomCode = RoomCode.DiningRoom;
		diningTableBreadth = diningTableBreadthRange.Random;
		diningTableLength = diningTableLengthRange.Random;
		diningTableMargin = diningTableMarginRange.Random;
		tileSet = (DiningRoomTileset)TileSet;
	}

	protected override void SetRoomDimensions()
	{
		vertical = (Random.value > 0.5f);
		int dim1 = diningTableMargin * 2 + diningTableBreadth;
		int dim2 = diningTableMargin * 2 + diningTableLength;
		if (vertical)
		{
			width = dim1; height = dim2;
		} else
		{
			width = dim2; height = dim1;
		}
		//Do this to not let the width and height get modified by dimension checking
		widthRange.m_Min = width;
		widthRange.m_Max = width;
		heightRange.m_Min = height;
		heightRange.m_Max = height;
	}

	public override void GenerateFurniture()
	{
		GenerateLightSwitch();
		GenerateDiningTable();
		GenerateChairs();
	}

	void GenerateDiningTable()
	{
		//Corners
		TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(x + diningTableMargin, y + diningTableMargin, 0), tileSet.diningTableBottomLeft);
		TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(x + width - diningTableMargin - 1, y + diningTableMargin, 0), tileSet.diningTableBottomRight);
		TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(x + diningTableMargin, y + height - diningTableMargin - 1, 0), tileSet.diningTableTopLeft);
		TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(x + width - diningTableMargin - 1, y + height - diningTableMargin - 1, 0), tileSet.diningTableTopRight);

		for (int xPos = x + diningTableMargin + 1; xPos < x + width - diningTableMargin - 1; xPos++)
		{
			TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(xPos, y + diningTableMargin, 0), tileSet.diningTableBottomCenter);
			TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(xPos, y + height - diningTableMargin - 1, 0), tileSet.diningTableTopCenter);
		}
		for (int yPos = y + diningTableMargin + 1; yPos < y + height - diningTableMargin - 1; yPos++)
		{
			TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(x + diningTableMargin, yPos, 0), tileSet.diningTableMidLeft);
			TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(x + width - diningTableMargin - 1, yPos, 0), tileSet.diningTableMidRight);
		}
		for (int xPos = x + diningTableMargin + 1; xPos < x + width - diningTableMargin - 1; xPos++)
		{
			for (int yPos = y + diningTableMargin + 1; yPos < y + height - diningTableMargin - 1; yPos++)
			{
				TileSetRegistry.I.floorTilemaps[story].SetTile(new Vector3Int(xPos, yPos, 0), tileSet.diningTableMidCenter);
			}
		}

		Vector2 position = new Vector2(x + width / 2f - .5f, y + height / 2f - .5f);
		Furniture tableCollider = InstantiateFurniture(PrefabRegistry.I.boxOverlay.GetComponent<Furniture>(), position);

		if (vertical)
			tableCollider.transform.localScale = new Vector2(diningTableBreadth, diningTableLength);
		else
			tableCollider.transform.localScale = new Vector2(diningTableLength, diningTableBreadth);
	}

	void GenerateChairs()
	{
		for (int xPos = x + diningTableMargin + 1; xPos < x + width - diningTableMargin - 1; xPos++)
		{
			float rng = Random.value;
			if (rng < chairGenerationChance)
			{
				Furniture chair = InstantiateFurniture(tileSet.chair, new Vector2(xPos, y + diningTableMargin - 1));
				if (chair)
				{
					chair.GetComponent<CardinalSprite>().UpdateDirection(Direction.South);
				}
			}
			rng = Random.value;
			if (rng < chairGenerationChance)
			{
				Furniture chair = InstantiateFurniture(tileSet.chair, new Vector2(xPos, y + height - diningTableMargin));
				if (chair)
				{
					chair.GetComponent<CardinalSprite>().UpdateDirection(Direction.South);
				}
			}
		}
		for (int yPos = y + diningTableMargin + 1; yPos < y + height - diningTableMargin - 1; yPos++)
		{
			float rng = Random.value;
			if (rng < chairGenerationChance)
			{
				Furniture chair = InstantiateFurniture(tileSet.chair, new Vector2(x + diningTableMargin - 1, yPos));
				if (chair)
				{
					chair.GetComponent<CardinalSprite>().UpdateDirection(Direction.East);
				}
			}
			rng = Random.value;
			if (rng < chairGenerationChance)
			{
				Furniture chair = InstantiateFurniture(tileSet.chair, new Vector2(x + width - diningTableMargin, yPos));
				if (chair)
				{
					chair.GetComponent<CardinalSprite>().UpdateDirection(Direction.West);
				}
			}
		}
	}
}
