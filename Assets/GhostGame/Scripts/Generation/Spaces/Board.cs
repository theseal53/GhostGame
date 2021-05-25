using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public int numStories;
    public int columns;
    public int rows;

    public StartingRoom startingRoom;
    public Direction startingDoorwayDirection;

    public List<Story> stories = new List<Story>();
    //public List<Room> rooms = new List<Room>();
    //public List<Corridor> corridors = new List<Corridor>();

    public sbyte[][][] tiles;

    public int roomMargin;
    public int boardMargin;

    public Board(int stories, int columns, int rows)
	{
        numStories = stories;
        this.columns = columns;
        this.rows = rows;
        SetupTilesArray();
	}

    private void SetupTilesArray()
    {
        tiles = new sbyte[numStories][][];
        for (int i = 0; i < numStories; i++)
        {
            tiles[i] = new sbyte[rows][];
            for (int j = 0; j < rows; j++)
            {
                tiles[i][j] = new sbyte[columns];
                for (int k = 0; k < columns; k++)
				{
                    tiles[i][j][k] = Constants.EMPTY_CODE;
                }
            }
        }
    }

}
