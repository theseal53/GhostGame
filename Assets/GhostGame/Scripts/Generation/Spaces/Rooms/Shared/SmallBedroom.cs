using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBedroom : Room
{
	protected Rect GenerateCornerBed(Furniture bedPrefab)
	{
		Orientation orientation = Random.value < .5 ? Orientation.Horizontal : Orientation.Vertical;

		int bedWidth = orientation == Orientation.Horizontal ? 3 : 2;
		int bedHeight = orientation == Orientation.Vertical ? 3 : 2;

		Vector2Int bottomLeftCorner = new Vector2Int(x, y);
		Vector2Int bottomRightCorner = new Vector2Int(x + width - bedWidth, y);
		Vector2Int topLeftCorner = new Vector2Int(x, y + height - bedHeight);
		Vector2Int topRightCorner = new Vector2Int(x + width - bedWidth, y + height - bedHeight);

		List<Vector2Int> availablePositions = new List<Vector2Int>();

		Rect rect = new Rect(0, 0, bedWidth, bedHeight);
		rect.position = bottomLeftCorner;
		if (!ObstructsDoorway(rect))
			availablePositions.Add(bottomLeftCorner);
		rect.position = bottomRightCorner;
		if (!ObstructsDoorway(rect))
			availablePositions.Add(bottomRightCorner);
		rect.position = topLeftCorner;
		if (!ObstructsDoorway(rect))
			availablePositions.Add(topLeftCorner);
		rect.position = topRightCorner;
		if (!ObstructsDoorway(rect))
			availablePositions.Add(topRightCorner);

		Vector2Int bedPosition = availablePositions[Random.Range(0, availablePositions.Count)];

		if (orientation == Orientation.Horizontal)
		{
			Furniture bed = InstantiateFurniture(bedPrefab, bedPosition);
			CardinalSprite cardinalSprite = bed.GetComponent<CardinalSprite>();
			if (cardinalSprite)
				cardinalSprite.direction = Random.value < .5 ? Direction.East : Direction.West;
		}
		else
		{
			Furniture bed = InstantiateFurniture(bedPrefab, bedPosition);
			CardinalSprite cardinalSprite = bed.GetComponent<CardinalSprite>();
			if (cardinalSprite)
				cardinalSprite.direction = Random.value < .5 ? Direction.North : Direction.South;
		}

		return new Rect(bedPosition.x, bedPosition.y, bedWidth, bedHeight);
	}
}
