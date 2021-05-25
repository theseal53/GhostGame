using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TilePositionParser
{
	private int numStories;
	private int numRows;
	private int numColumns;

	private TilePosition[][][] SetupTilesArray(int stories, int rows, int columns)
	{
		TilePosition[][][] tiles = new TilePosition[stories][][];
		for (int i = 0; i < stories; i++)
		{
			tiles[i] = new TilePosition[rows][];
			for (int j = 0; j < rows; j++)
			{
				tiles[i][j] = new TilePosition[columns];
			}
		}
		return tiles;
	}

	public TilePosition[][][] Parse(sbyte[][][] tileTypes)
	{
		numStories = tileTypes.Length;
		numRows = tileTypes[0].Length;
		numColumns = tileTypes[0][0].Length;

		TilePosition[][][] tilePositions = SetupTilesArray(numStories, numRows, numColumns);

		for (int i = 0; i < numStories; i++)
		{
			for (int j = 0; j < numRows; j++)
			{
				for (int k = 0; k < numColumns; k++)
				{
					tilePositions[i][j][k] = DetermineCellPosition(i, j, k, tileTypes);
				}
			}
		}
		return tilePositions;
	}

	private TilePosition DetermineCellPosition(int story, int row, int column, sbyte[][][] tileTypes)
	{
	bool isWall = IsWall(tileTypes[story][row][column]);
	//This will only work if there aren't any 1-width tiles
		if (row > 0 && IsWall(tileTypes[story][row - 1][column]) != isWall)
		{
			if (row < numRows - 1 && IsWall(tileTypes[story][row + 1][column]) != isWall)
			{
				return TilePosition.HorizontalHall;
			}
				if (column > 0 && IsWall(tileTypes[story][row][column - 1]) != isWall)
			{
				return TilePosition.BottomLeft;
			}
			else if (column < numColumns - 1 && IsWall(tileTypes[story][row][column + 1]) != isWall)
			{
				return TilePosition.BottomRight;
			}
			else
			{
				return TilePosition.BottomCenter;
			}
		}
		else if (row < numRows - 1 && IsWall(tileTypes[story][row + 1][column]) != isWall)
		{
			if (column > 0 && IsWall(tileTypes[story][row][column - 1]) != isWall)
			{
				return TilePosition.TopLeft;
			}
			else if (column < numColumns - 1 && IsWall(tileTypes[story][row][column + 1]) != isWall)
			{
				return TilePosition.TopRight;
			}
			else
			{
				return TilePosition.TopCenter;
			}
		}
		else if (column > 0 && IsWall(tileTypes[story][row][column - 1]) != isWall)
		{
			if (column < numColumns - 1 && IsWall(tileTypes[story][row][column + 1]) != isWall)
			{
				return TilePosition.VerticalHall;
			}
			return TilePosition.MidLeft;
		}
		else if (column < numColumns - 1 && IsWall(tileTypes[story][row][column + 1]) != isWall)
		{
			return TilePosition.MidRight;
		}
		else
		{
			return TilePosition.MidCenter;
		}
	}

	private bool IsWall(sbyte code)
	{
		return code == Constants.EMPTY_CODE;
	}

	/*private TilePosition DetermineWallTile(int story, int row, int column, sbyte[][][] tileTypes)
	{
		if (
			(row > 0 && tileTypes[story][row - 1][column] != Constants.EMPTY_CODE) ||
			(column > 0 && tileTypes[story][row][column - 1] != Constants.EMPTY_CODE) ||
			(row < numRows - 1 && tileTypes[story][row + 1][column] != Constants.EMPTY_CODE) ||
			(column < numColumns - 1 && tileTypes[story][row][column + 1] != Constants.EMPTY_CODE)
			)
		{
			return TilePosition.Wall;
		}
		return TilePosition.Empty;
	}*/
}
