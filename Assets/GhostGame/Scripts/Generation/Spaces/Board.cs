using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public int columns;
    public int rows;

    public StartingRoom startingRoom;

    public List<Room> rooms = new List<Room>();
    public List<Corridor> corridors = new List<Corridor>();

    public int[][] tiles;
    public TilePosition[][] tilePositions;

    public int roomMargin;
    public int boardMargin;

    public Board(int columns, int rows)
	{
        this.columns = columns;
        this.rows = rows;
        SetupTilesArray();
	}

    private void SetupTilesArray()
    {
        tiles = new int[rows][];

        for (int i = 0; i < tiles.Length; ++i)
        {
            tiles[i] = new int[columns];
			for (int j = 0; j < columns; j++)
			{
                tiles[i][j] = Constants.EMPTY_CODE;
			}
        }
    }

}
