﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bathroom : Room
{

	BathroomTileset tileSet;

	public Bathroom() : base()
	{
		roomCode = RoomCode.Bathroom;
		widthRange = new IntRange(5, 6);
		heightRange = new IntRange(5, 6);
		tileSet = (BathroomTileset)TileSet;
	}

	public override bool CanMakeMoreDoors()
	{
		return false;
	}

	public override void GenerateFurniture()
	{
		List<Vector2> open2x2Positions = new List<Vector2>();
		if (!ObstructsDoorway(x, y, 2, 2))
			open2x2Positions.Add(new Vector2(x, y));
		if (!ObstructsDoorway(x + width - 2, y, 2, 2))
			open2x2Positions.Add(new Vector2(x + width - 2, y));
		if (!ObstructsDoorway(x, y + height - 2, 2, 2))
			open2x2Positions.Add(new Vector2(x, y + height - 2));
		if (!ObstructsDoorway(x + width - 2, y + height - 2, 2, 2))
			open2x2Positions.Add(new Vector2(x + width - 2, y + height - 2));
		
		int index = Random.Range(0, open2x2Positions.Count);
		GameObject bathtub = Object.Instantiate(tileSet.bathtub);
		bathtub.transform.position = open2x2Positions[index];
		Rect bathtubRect = new Rect(open2x2Positions[index], new Vector2(2, 2));
		open2x2Positions.RemoveAt(index);

		index = Random.Range(0, open2x2Positions.Count);
		GameObject cabinet = Object.Instantiate(tileSet.cabinet);
		Rect cabinetRect = new Rect(open2x2Positions[index], new Vector2(2, 2));
		cabinet.transform.position = open2x2Positions[index];

		List<Vector2> open1x1Positions = new List<Vector2>();
		for (int xPos = x + 1; xPos < x + width - 1; xPos++)
		{
			Vector2 position = new Vector2(xPos, y);
			Rect rect = new Rect(xPos, y, 1, 1);
			if (!ObstructsDoorway(rect) && !rect.Overlaps(bathtubRect) && !rect.Overlaps(cabinetRect))
				open1x1Positions.Add(position);
			position = new Vector2(xPos, y + height - 1);
			rect = new Rect(xPos, y + height - 1, 1, 1);
			if (!ObstructsDoorway(rect) && !rect.Overlaps(bathtubRect) && !rect.Overlaps(cabinetRect))
				open1x1Positions.Add(position);
		}
		for (int yPos = y + 1; yPos < y + height - 1; yPos++)
		{
			Vector2 position = new Vector2(x, yPos);
			Rect rect = new Rect(x, yPos, 1, 1);
			if (!ObstructsDoorway(rect) && !rect.Overlaps(bathtubRect) && !rect.Overlaps(cabinetRect))
				open1x1Positions.Add(position);
			position = new Vector2(x + width - 1,yPos);
			rect = new Rect(x + width - 1, yPos, 1, 1);
			if (!ObstructsDoorway(rect) && !rect.Overlaps(bathtubRect) && !rect.Overlaps(cabinetRect))
				open1x1Positions.Add(position);
		}

		index = Random.Range(0, open1x1Positions.Count);
		GameObject sink = Object.Instantiate(tileSet.sink);
		sink.transform.position = open1x1Positions[index];
		open1x1Positions.RemoveAt(index);

		index = Random.Range(0, open1x1Positions.Count);
		GameObject toilet = Object.Instantiate(tileSet.toilet);
		toilet.transform.position = open1x1Positions[index];
		open1x1Positions.RemoveAt(index);

		index = Random.Range(0, open1x1Positions.Count);
		GameObject wasteBasket = Object.Instantiate(tileSet.wasteBasket);
		wasteBasket.transform.position = open1x1Positions[index];
		open1x1Positions.RemoveAt(index);
	}
}
