using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TilePositionParser
{
	private int numRows;
	private int numColumns;


	private TilePosition[,] SetupTilesArray(int rows, int columns)
	{
		TilePosition[,] tiles = new TilePosition[columns, rows];
		return tiles;
	}

	public TilePosition[,] Parse(short[,] tileTypes)
	{
		numRows = tileTypes.GetLength(0);
		numColumns = tileTypes.GetLength(1);

		TilePosition[,] tilePositions = SetupTilesArray(numRows, numColumns);

		for (int i = 0; i < numRows; i++)
		{
			for (int j = 0; j < numColumns; j++)
			{
				tilePositions[i,j] = DetermineCellPosition(i, j, tileTypes);
			}
		}
		return tilePositions;
	}

	private TilePosition DetermineCellPosition(int row, int column, short[,] tileTypes)
	{
		//This will only work if there aren't any 1-width tiles
		if (tileTypes[row, column] != Constants.EMPTY_CODE) {
			if (row == 0 || tileTypes[row - 1, column] == Constants.EMPTY_CODE)
			{
				if (row == numRows - 1 || tileTypes[row + 1, column] == Constants.EMPTY_CODE)
				{
					return TilePosition.HorizontalHall;
				}
					if (column == 0 || tileTypes[row, column - 1] == Constants.EMPTY_CODE)
				{
					return TilePosition.BottomLeft;
				}
				else if (column == numColumns - 1 || tileTypes[row, column + 1] == Constants.EMPTY_CODE)
				{
					return TilePosition.BottomRight;
				}
				else
				{
					return TilePosition.BottomCenter;
				}
			}
			else if (row == numRows - 1 || tileTypes[row + 1, column] == Constants.EMPTY_CODE)
			{
				if (column == 0 || tileTypes[row, column - 1] == Constants.EMPTY_CODE)
				{
					return TilePosition.TopLeft;
				}
				else if (column == numColumns - 1 || tileTypes[row, column + 1] == Constants.EMPTY_CODE)
				{
					return TilePosition.TopRight;
				}
				else
				{
					return TilePosition.TopCenter;
				}
			}
			else if (column == 0 || tileTypes[row, column - 1] == Constants.EMPTY_CODE)
			{
				if (column == numColumns - 1 || tileTypes[row, column + 1] == Constants.EMPTY_CODE)
				{
					return TilePosition.VerticalHall;
				}
				return TilePosition.MidLeft;
			}
			else if (column == numColumns - 1 || tileTypes[row, column + 1] == Constants.EMPTY_CODE)
			{
				return TilePosition.MidRight;
			}
			else
			{
				return TilePosition.MidCenter;
			}
		} else
		{
			return DetermineWallTile(row, column, tileTypes);
		}
	}

	private TilePosition DetermineWallTile(int row, int column, short[,] tileTypes)
	{
		if (
			(row > 0 && tileTypes[row - 1, column] != Constants.EMPTY_CODE) ||
			(column > 0 && tileTypes[row, column - 1] != Constants.EMPTY_CODE) ||
			(row < numRows - 1 && tileTypes[row + 1, column] != Constants.EMPTY_CODE) ||
			(column < numColumns - 1 && tileTypes[row, column + 1] != Constants.EMPTY_CODE)
			)
		{
			return TilePosition.Wall;
		}
		return TilePosition.Empty;
	}
}
