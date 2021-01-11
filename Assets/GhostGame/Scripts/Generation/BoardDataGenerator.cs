using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDataGenerator
{
    public int columns = 50;
    public int rows = 50;
    public IntRange numRooms = new IntRange(10,10);
    public IntRange corridorLength = new IntRange(3, 5);

    private int startingDoorwayMargin = 2;
    private int roomMargin = 1;

    TilePositionParser tilePositionParser;

    int failedRoomsAllowed = 100;

    public Board board;

    // Start is called before the first frame update
    public Board GenerateBoardData()
    {
        board = new Board(columns, rows);
        board.roomMargin = roomMargin;
        tilePositionParser = new TilePositionParser();
        CreateRoomsAndCorridors();
        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();
        board.tilePositions = tilePositionParser.Parse(board.tiles);
        return board;
    }

    private void CreateRoomsAndCorridors()
    {
        int failedRoomAttempts = 0;
        int roomsToGenerate = numRooms.Random;
        board.startingRoom = new StartingRoom();
        board.rooms.Add(board.startingRoom);
        Doorway startingDoorway = GenerateStartingDoorway();

        //Make and shuffle room ids
        List<int> roomIds = new List<int>();
        for (int i = 1; i < roomsToGenerate; i++)
        {
            roomIds.Insert(Random.Range(0, i), i);
        }

        board.rooms[0].SetupRoom(board, startingDoorway, 0);

        for (int i = 0; i < roomsToGenerate-1; i++)
        {
            if (failedRoomAttempts >= failedRoomsAllowed)
			{
                Debug.Log("Failed room total reached");
                break;
			}
            Room stemRoom = SelectStemRoom();
            Doorway possibleDoorway = stemRoom.PossibleDoorway();

            Corridor corridorAttempt = new Corridor();
            corridorAttempt.SetupCorridor(possibleDoorway, board, 4);

            Room roomAttempt = new Room();
            roomAttempt.SetupRoom(board, corridorAttempt.door2, roomIds[i]);

            bool valid = roomAttempt.TestRoomValidity(board);

            //Do testing
            if (valid) {
                stemRoom.doorways.Add(possibleDoorway);
                board.corridors.Add(corridorAttempt);
                board.rooms.Add(roomAttempt);
            } else
			{
                i--;
                failedRoomAttempts++;
			}
        }
    }

    public Doorway GenerateStartingDoorway()
	{
        int rng = Random.Range(0, 4);
        switch(rng)
		{
            case 0:
                return new Doorway(board.columns / 2, startingDoorwayMargin, Direction.South, true);
            case 1:
                return new Doorway(board.columns / 2, board.rows - startingDoorwayMargin, Direction.North, true);
            case 2:
                return new Doorway(startingDoorwayMargin, board.rows / 2, Direction.West, true);
            default:
                return new Doorway(board.columns - startingDoorwayMargin, board.rows / 2, Direction.East, true);

        }
	}

    private void SetTilesValuesForRooms()
    {
        for (int i = 0; i < board.rooms.Count; i++)
        {
            try
			{
                board.rooms[i].PrintToTilesArray(board.tiles);
            }catch(System.Exception)
			{
                Debug.LogError("Failed tileprint of " + board.rooms[i].ToString());
			}
        }
    }
    private void SetTilesValuesForCorridors()
    {
        for (int i = 0; i < board.corridors.Count; i++)
        {
            try
			{
                board.corridors[i].PrintToTilesArray(board.tiles);
            }
            catch (System.Exception)
            {
                Debug.LogError("Failed tileprint of " + board.corridors[i].ToString());
            }
        }
    }

    private Room SelectStemRoom()
	{
        return board.rooms[Random.Range(0, board.rooms.Count)];
	}

    private void PrintAllRooms()
	{
		for (int i = 0; i < board.rooms.Count; i++)
		{
            Debug.Log(board.rooms[i].ToString());
		}
	}
}
