using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Library : Room
{
	private int bookcaseMargin = 3;
	private int aisleSpacing = 2;

	LibraryTileset tileSet;

	List<float> outerChances;
	List<FurnitureDimensionSet> outerChoices;

	public Library() : base()
	{
		roomCode = RoomCode.Library;
		widthRange = new IntRange(10, 20);
		heightRange = new IntRange(10, 20);
		tileSet = (LibraryTileset)TileSetRegistry.I.GetTileSet(roomCode);

		outerChances = new List<float>() {
			2,
			1,
			4,
			5,
			2
		};

		outerChoices = new List<FurnitureDimensionSet>() {
			new FurnitureDimensionSet(tileSet.OneByOneObstacle, 1),
			new FurnitureDimensionSet(tileSet.TwoByTwoObstacle, 2),
			new FurnitureDimensionSet(tileSet.OneByTwoBookCase, 1),
			new FurnitureDimensionSet(null, 1),
			new FurnitureDimensionSet(null, 2)
		};
	}

	public override void GenerateFurniture()
	{
		GenerateLightSwitch();
		GenerateOuterObstacles();
		GenerateInnerBookcases();
	}

	private void GenerateOuterObstacles()
	{
		int cornerOffset = 2;
		//Bottom Left corner
		FurnitureDimensionSet dimensionSet = GetRandomOuterGameObject();
		if (dimensionSet.target != null)
		{
			AttemptFurnitureInstantiation(dimensionSet.target, x, y, dimensionSet.breadth, dimensionSet.breadth);
		}
		//Bottom Right corner
		dimensionSet = GetRandomOuterGameObject();
		if (dimensionSet.target != null)
		{
			AttemptFurnitureInstantiation(dimensionSet.target, x + width - dimensionSet.breadth, y, dimensionSet.breadth, dimensionSet.breadth);
		}
		//Top Left corner
		dimensionSet = GetRandomOuterGameObject();
		if (dimensionSet.target != null)
		{
			AttemptFurnitureInstantiation(dimensionSet.target, x, y + height - dimensionSet.breadth, dimensionSet.breadth, dimensionSet.breadth);
		}
		//Top Right corner
		dimensionSet = GetRandomOuterGameObject();
		if (dimensionSet.target != null)
		{
			AttemptFurnitureInstantiation(dimensionSet.target, x + width - dimensionSet.breadth, y + height - dimensionSet.breadth, dimensionSet.breadth, dimensionSet.breadth);
		}

		//Bottom wall
		{
			int xPos = x + cornerOffset;
			dimensionSet = GetRandomOuterGameObject();
			while (xPos < x + width - cornerOffset - dimensionSet.breadth)
			{
				//Sorry for this monstrosity. It's to ensure that if a 2x2 piece is created, it won't spawn if it can block off a corner
				if (!(dimensionSet.breadth == 2 && (xPos == x + cornerOffset || xPos == x + width - cornerOffset - dimensionSet.breadth)))
				{
					int yPos = y;
					if (dimensionSet.target != null)
						AttemptFurnitureInstantiation(dimensionSet.target, xPos, yPos, dimensionSet.breadth, dimensionSet.breadth);
				}
				xPos += dimensionSet.breadth;
				dimensionSet = GetRandomOuterGameObject();
			}
		}
		//Top wall
		{
			int xPos = x + cornerOffset;
			dimensionSet = GetRandomOuterGameObject();
			while (xPos < x + width - cornerOffset - dimensionSet.breadth)
			{
				if (!(dimensionSet.breadth == 2 && (xPos == x + cornerOffset || xPos == x + width - cornerOffset - dimensionSet.breadth)))
				{
					int yPos = y + height - dimensionSet.breadth;
					if (dimensionSet.target != null)
						AttemptFurnitureInstantiation(dimensionSet.target, xPos, yPos, dimensionSet.breadth, dimensionSet.breadth);
				}
				xPos += dimensionSet.breadth;
				dimensionSet = GetRandomOuterGameObject();
			}
		}
		//Left wall
		{
			int yPos = y + cornerOffset;
			dimensionSet = GetRandomOuterGameObject();
			while (yPos < y + height - cornerOffset - dimensionSet.breadth)
			{
				if (!(dimensionSet.breadth == 2 && (yPos == y + cornerOffset || yPos == y + height - cornerOffset - dimensionSet.breadth)))
				{
					int xPos = x;
					if (dimensionSet.target != null)
						AttemptFurnitureInstantiation(dimensionSet.target, xPos, yPos, dimensionSet.breadth, dimensionSet.breadth);
				}
				yPos += dimensionSet.breadth;
				dimensionSet = GetRandomOuterGameObject();
			}
		}
		//Right wall
		{
			int yPos = y + cornerOffset;
			dimensionSet = GetRandomOuterGameObject();
			while (yPos < y + height - cornerOffset - dimensionSet.breadth)
			{
				if (!(dimensionSet.breadth == 2 && (yPos == y + cornerOffset || yPos == y + height - cornerOffset - dimensionSet.breadth)))
				{
					int xPos = x + width - dimensionSet.breadth;
					if (dimensionSet.target != null)
						AttemptFurnitureInstantiation(dimensionSet.target, xPos, yPos, dimensionSet.breadth, dimensionSet.breadth);
				}
				yPos += dimensionSet.breadth;
				dimensionSet = GetRandomOuterGameObject();
			}
		}
	}


	private void GenerateInnerBookcases()
	{
		int given = bookcaseMargin * 2 + 2; // +2 is the width of each bookcase;
		int increment = 2 + aisleSpacing;
		int variableSpace = width - given;
		int extraSpace = variableSpace % increment;
		int offset = (extraSpace + Random.Range(0,1)) / 2; //Add the random space to alternate how it is rounded

		bool vertical = (Random.value > 0.5f);
		if (vertical)
		{
			int xPos = x + bookcaseMargin + offset;
			while (xPos < x + width - bookcaseMargin - 1) //Minus 1, to account for the extra space the bookcase takes
			{
				int yPos = y + bookcaseMargin + offset;
				while (yPos < y + height - bookcaseMargin - 1)
				{
					InstantiateFurniture(tileSet.TwoByThreeBookCase, new Vector2(xPos, yPos));
					yPos += 2;
				}
				xPos += 2 + aisleSpacing;
			}
		}
		else
		{
			int yPos = y + bookcaseMargin + offset;
			while (yPos < y + height - bookcaseMargin - 1) //Minus 1, to account for the extra space the bookcase takes
			{
				int xPos = x + bookcaseMargin + offset;
				while (xPos < x + width - bookcaseMargin - 1)
				{
					InstantiateFurniture(tileSet.TwoByThreeBookCase, new Vector2(xPos, yPos));
					xPos += 2;
				}
				yPos += 2 + aisleSpacing;
			}
		}
	}


	//Last is two space chance;

	public FurnitureDimensionSet GetRandomOuterGameObject()
	{
		return WeightedChoice.Choose<FurnitureDimensionSet>(outerChoices, outerChances);
	}
}
