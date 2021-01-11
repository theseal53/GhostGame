using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	North, East, South, West
}

public static class DirectionReverse
{
	public static Direction reverse(Direction direction)
	{
		return (Direction)(((int)direction + 2) % 4);
	}
}