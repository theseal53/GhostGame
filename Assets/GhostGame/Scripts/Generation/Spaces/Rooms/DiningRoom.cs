using System.Collections;
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
	public override TileSet TileSet
	{
		get
		{
			return tileSet;
		}
	}

	public DiningRoom() : base()
	{
		diningTableBreadth = diningTableBreadthRange.Random;
		diningTableLength = diningTableLengthRange.Random;
		diningTableMargin = diningTableMarginRange.Random;
		tileSet = TileSetRegistry.I.DiningRoom;
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

	public override void SetupRoom(Board board, Doorway doorway, int roomId)
	{
		base.SetupRoom(board, doorway, roomId);
	}

	public override void GenerateFurniture()
	{
		GenerateDiningTable();
		GenerateChairs();
	}

	void GenerateDiningTable()
	{
		//Corners
		TileSetRegistry.I.floorTilemap.SetTile(new Vector3Int(x + diningTableMargin, y + diningTableMargin, 0), tileSet.diningTableBottomLeft);
		TileSetRegistry.I.floorTilemap.SetTile(new Vector3Int(x + width - diningTableMargin - 1, y + diningTableMargin, 0), tileSet.diningTableBottomRight);
		TileSetRegistry.I.floorTilemap.SetTile(new Vector3Int(x + diningTableMargin, y + height - diningTableMargin - 1, 0), tileSet.diningTableTopLeft);
		TileSetRegistry.I.floorTilemap.SetTile(new Vector3Int(x + width - diningTableMargin - 1, y + height - diningTableMargin - 1, 0), tileSet.diningTableTopRight);

		for (int xPos = x + diningTableMargin + 1; xPos < x + width - diningTableMargin - 1; xPos++)
		{
			TileSetRegistry.I.floorTilemap.SetTile(new Vector3Int(xPos, y + diningTableMargin, 0), tileSet.diningTableBottomCenter);
			TileSetRegistry.I.floorTilemap.SetTile(new Vector3Int(xPos, y + height - diningTableMargin - 1, 0), tileSet.diningTableTopCenter);
		}
		for (int yPos = y + diningTableMargin + 1; yPos < y + height - diningTableMargin - 1; yPos++)
		{
			TileSetRegistry.I.floorTilemap.SetTile(new Vector3Int(x + diningTableMargin, yPos, 0), tileSet.diningTableMidLeft);
			TileSetRegistry.I.floorTilemap.SetTile(new Vector3Int(x + width - diningTableMargin - 1, yPos, 0), tileSet.diningTableMidRight);
		}
		for (int xPos = x + diningTableMargin + 1; xPos < x + width - diningTableMargin - 1; xPos++)
		{
			for (int yPos = y + diningTableMargin + 1; yPos < y + height - diningTableMargin - 1; yPos++)
			{
				TileSetRegistry.I.floorTilemap.SetTile(new Vector3Int(xPos, yPos, 0), tileSet.diningTableTopCenter);
			}
		}
		GameObject gameObject = new GameObject();
		gameObject.transform.position = new Vector2(x + width / 2f - .5f, y + height / 2f - .5f);
		BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
		if (vertical)
			collider.size = new Vector2(diningTableBreadth, diningTableLength);
		else
			collider.size = new Vector2(diningTableLength, diningTableBreadth);
	}

	void GenerateChairs()
	{
		for (int xPos = x + diningTableMargin + 1; xPos < x + width - diningTableMargin - 1; xPos++)
		{
			float rng = Random.value;
			if (rng < chairGenerationChance)
			{
				GameObject newChair = Object.Instantiate(tileSet.OneByOneObstacle);
				newChair.transform.position = new Vector2(xPos, y + diningTableMargin - 1);
			}
			rng = Random.value;
			if (rng < chairGenerationChance)
			{
				GameObject newChair = Object.Instantiate(tileSet.OneByOneObstacle);
				newChair.transform.position = new Vector2(xPos, y + height - diningTableMargin);
			}
		}
		for (int yPos = y + diningTableMargin + 1; yPos < y + height - diningTableMargin - 1; yPos++)
		{
			float rng = Random.value;
			if (rng < chairGenerationChance)
			{
				GameObject newChair = Object.Instantiate(tileSet.OneByOneObstacle);
				newChair.transform.position = new Vector2(x + diningTableMargin - 1, yPos);
			}
			rng = Random.value;
			if (rng < chairGenerationChance)
			{
				GameObject newChair = Object.Instantiate(tileSet.OneByOneObstacle);
				newChair.transform.position = new Vector2(x + width - diningTableMargin, yPos);
			}
		}
	}
}
