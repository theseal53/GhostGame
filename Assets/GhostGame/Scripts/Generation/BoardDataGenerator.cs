using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDataGenerator
{
    public IntRange numRooms = new IntRange(10, 10);
    public IntRange corridorLength = new IntRange(3, 5);

    private int startingDoorwayMargin = 2;
    private int startingRoomCorridorBreadth = 3;
    private int boardMargin = 1;
    private int roomMargin = 1;


    int failedRoomsAllowed = 100;

    public Board board;

    List<int> roomRngIdentifiers = new List<int>();

    // Start is called before the first frame update
    public Board GenerateBoardData(int columns, int rows)
    {
        board = new Board(columns, rows);
        board.roomMargin = roomMargin;
        board.boardMargin = boardMargin;
        CreateRoomsAndCorridors();
        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();
        return board;
    }

    private void CreateRoomsAndCorridors()
    {
        int failedRoomAttempts = 0;
        int roomsToGenerate = numRooms.Random;
        board.startingRoom = new StartingRoom();
        board.rooms.Add(board.startingRoom);
        Doorway startingDoorway = GenerateStartingDoorway();

        InitRngIdentifiers(roomsToGenerate);

        //Make and shuffle room ids

        board.rooms[0].SetupRoom(board, startingDoorway);

        for (int i = 1; i < roomsToGenerate; i++)
        {
            if (failedRoomAttempts >= failedRoomsAllowed)
			{
                Debug.Log("Failed room total reached");
                break;
			}
            Room stemRoom = SelectStemRoom();
            if (stemRoom.CanMakeMoreDoors())
            {
                Doorway possibleDoorway = stemRoom.PossibleDoorway();

                Corridor corridorAttempt = new Corridor();
                corridorAttempt.SetupCorridor(possibleDoorway, board, 0);

                Room roomAttempt = RandomRoom();
                roomAttempt.SetupRoom(board, corridorAttempt.door2);

                bool valid = roomAttempt.TestRoomValidity(board);

                //Do testing
                if (valid)
                {
                    roomAttempt.PostValiditySetup();
                    stemRoom.doorways.Add(possibleDoorway);
                    board.corridors.Add(corridorAttempt);
                    board.rooms.Add(roomAttempt);
                    roomRngIdentifiers.RemoveAt(0);
                }
                else
                {
                    i--;
                    failedRoomAttempts++;
                }
            } else
			{
                i--;
                failedRoomAttempts++;
			}
        }
    }

    private void InitRngIdentifiers(int roomsToGenerate)
	{
        //Go to roomsToGenerate - 1, because we already are making the starting room
        for (int i = 0; i < roomsToGenerate - 1; i++)
        {
            roomRngIdentifiers.Insert(UnityEngine.Random.Range(0, i), i);
        }
    }

    private Room RandomRoom()
	{
        int rng = roomRngIdentifiers[0];
		switch (rng)
		{
            case 0:
                return new Library();
            case 1:
                return new DiningRoom();
            case 2:
                return new Bathroom();
            case 3:
                return new Kitchen();
            default:
                return new Room();
		}
	}

    public Doorway GenerateStartingDoorway()
	{
        int rng = UnityEngine.Random.Range(0, 4);
        switch(rng)
		{
            case 0:
                return new Doorway(board.columns / 2, startingDoorwayMargin, Direction.South, Constants.DEFAULT_DOOR_BREADTH, true);
            case 1:
                return new Doorway(board.columns / 2, board.rows - startingDoorwayMargin, Direction.North, Constants.DEFAULT_DOOR_BREADTH, true);
            case 2:
                return new Doorway(startingDoorwayMargin, board.rows / 2, Direction.West, Constants.DEFAULT_DOOR_BREADTH, true);
            default:
                return new Doorway(board.columns - startingDoorwayMargin, board.rows / 2, Direction.East, Constants.DEFAULT_DOOR_BREADTH, true);

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
        return board.rooms[UnityEngine.Random.Range(0, board.rooms.Count)];
	}

    private void PrintAllRooms()
	{
		for (int i = 0; i < board.rooms.Count; i++)
		{
            Debug.Log(board.rooms[i].ToString());
		}
	}
}
