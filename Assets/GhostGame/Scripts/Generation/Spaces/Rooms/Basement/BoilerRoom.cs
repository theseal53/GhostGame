using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilerRoom : Room
{
	private BoilerRoomTileset tileset;

	private int borderMargin = 3;
	private int boilerSmallBreadth = 2;
	private int boilerLargeBreadth = 4;
	private int pipeValveBreadth = 1;
	private int switchBoxBreadth = 2;
	private int borderPipeBreadth = 1;

	private int allowedInstantiationFails = 20;

	List<Furniture> borderOptions;
	List<float> borderChances;

	public BoilerRoom() : base()
	{
		widthRange = new IntRange(10, 12);
		heightRange = new IntRange(10, 12);
		roomCode = RoomCode.BoilerRoom;
		tileset = (BoilerRoomTileset)TileSetRegistry.I.GetTileSet(roomCode);

		borderOptions = new List<Furniture>()
		{
			tileset.pipeTangle1,
			tileset.pipeTangle2,
			null
		};
		borderChances = new List<float>()
		{
			.2f,
			.2f,
			.6f,
		};
	}

	public override void GenerateFurniture()
	{
		GenerateLightSwitch();
		GenerateBorderPipes();
		GenerateCenterFurniture();
	}

	private void GenerateBorderPipes()
	{
		//Sides
		float precompTotal = WeightedChoice.PrecompTotal(borderChances);
		Furniture prefab;
		//Horizontal sides and corners
		for (int xPos = x; xPos < x + width; xPos++)
		{
			prefab = RandomBorderItem(precompTotal);
			Vector2 position = new Vector3(xPos, y);
			if (prefab != null)
			{
				Rect rect = new Rect(position.x, position.y, borderPipeBreadth, borderPipeBreadth);
				AttemptFurnitureInstantiation(prefab, rect);
			}
			prefab = RandomBorderItem(precompTotal);
			position = new Vector3(xPos, y + height - 1);
			if (prefab != null)
			{
				Rect rect = new Rect(position.x, position.y, borderPipeBreadth, borderPipeBreadth);
				AttemptFurnitureInstantiation(prefab, rect);
			}
		}
		//Vertical sides
		for (int yPos = y + 1; yPos < y + height - 1; yPos++)
		{
			prefab = RandomBorderItem(precompTotal);
			Vector2 position = new Vector3(x, yPos);
			if (prefab != null)
			{
				Rect rect = new Rect(position.x, position.y, borderPipeBreadth, borderPipeBreadth);
				AttemptFurnitureInstantiation(prefab, rect);
			}
			prefab = RandomBorderItem(precompTotal);
			position = new Vector3(x + width - 1, yPos);
			if (prefab != null)
			{
				Rect rect = new Rect(position.x, position.y, borderPipeBreadth, borderPipeBreadth);
				AttemptFurnitureInstantiation(prefab, rect);
			}
		}
	}
	private void GenerateCenterFurniture()
	{
		Orientation boilerOrientation = Random.value < .5 ? Orientation.Horizontal : Orientation.Vertical;
		Rect boilerRect;
		if (boilerOrientation == Orientation.Horizontal)
		{
			Vector2 boilerPosition = new Vector2(
				Random.Range(x + borderMargin, x + width - borderMargin - boilerLargeBreadth),
				Random.Range(y + borderMargin, y + height - borderMargin - boilerSmallBreadth));
			InstantiateFurniture(tileset.boilerHorizontal, boilerPosition);
			boilerRect = new Rect(boilerPosition.x, boilerPosition.y, boilerLargeBreadth, boilerSmallBreadth);
		}
		else { // Orientation.Vertical 
			Vector2 boilerPosition = new Vector2(
				Random.Range(x + borderMargin, x + width - borderMargin - boilerSmallBreadth),
				Random.Range(y + borderMargin, y + height - borderMargin - boilerLargeBreadth));
			InstantiateFurniture(tileset.boilerVertical, boilerPosition);
			boilerRect = new Rect(boilerPosition.x, boilerPosition.y, boilerSmallBreadth, boilerLargeBreadth);
		}

		int totalFails = 0;
		Rect switchBoxRect = new Rect();
		while (totalFails < allowedInstantiationFails)
		{
			switchBoxRect = new Rect(
				Random.Range(x + borderMargin, x + width - borderMargin - switchBoxBreadth),
				Random.Range(y + borderMargin, y + height - borderMargin - switchBoxBreadth),
				switchBoxBreadth,
				switchBoxBreadth);
			if (!switchBoxRect.Overlaps(boilerRect))
			{
				InstantiateFurniture(tileset.switchBox, switchBoxRect.position);
				break;
			}
			totalFails++;
		}
		while (totalFails < allowedInstantiationFails)
		{
			Rect pipeValveRect = new Rect(
				Random.Range(x + borderMargin, x + width - borderMargin - pipeValveBreadth),
				Random.Range(y + borderMargin, y + height - borderMargin - pipeValveBreadth),
				pipeValveBreadth,
				pipeValveBreadth);
			if (!pipeValveRect.Overlaps(boilerRect) && !pipeValveRect.Overlaps(switchBoxRect))
			{
				InstantiateFurniture(tileset.pipeValve, pipeValveRect.position);
				break;
			}
			totalFails++;
		}
	}

	private Furniture RandomBorderItem(float precompTotal)
	{
		return WeightedChoice.Choose(borderOptions, borderChances, precompTotal);
	}
}
