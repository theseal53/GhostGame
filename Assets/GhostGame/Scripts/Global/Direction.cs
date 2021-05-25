using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	North, East, South, West
}

public enum Orientation
{
	Vertical, Horizontal
}

public enum Verticality
{
	Up,
	Down
}

public static class DirectionUtil
{
	public static Direction Reverse(Direction direction)
	{
		return (Direction)(((int)direction + 2) % 4);
	}
	public static Orientation Reverse(Orientation orientation)
	{
		return orientation == global::Orientation.Vertical ? global::Orientation.Horizontal : global::Orientation.Vertical;
	}
	public static Orientation Orientation(Direction direction)
	{
		return (Orientation)((int)direction % 2);
	}
}