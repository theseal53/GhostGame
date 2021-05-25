using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Story
{

    public int failedRoomsAllowed = 100;
    protected List<int> roomRngIdentifiers = new List<int>();

    public IntRange numRooms = new IntRange(10, 10);

    protected int identifier;

    public List<Room> rooms = new List<Room>();
    public List<Corridor> corridors = new List<Corridor>();

    private int startingDoorwayMargin = 2;

    public Landing landing;

    public Story(int identifier)
	{
        this.identifier = identifier;
	}


    protected abstract Room RandomRoom();

    protected virtual void GenerateLandingRoom(Board board, Vector2Int stairway, Verticality verticality)
    {
        landing = new Landing();
        rooms.Add(landing);
        landing.SetupRoom(stairway, verticality, board.startingDoorwayDirection, identifier);
        landing.TestRoomValidity(board);
    }

    public virtual void CreateRoomsAndCorridors(Board board, Vector2Int stairway, Verticality verticality)
    {
        GenerateLandingRoom(board, stairway, verticality);
        GenerateRemainingRooms(board);
    }

    protected void GenerateRemainingRooms(Board board)
    {
        int failedRoomAttempts = 0;
        int roomsToGenerate = numRooms.Random;

        InitRngIdentifiers(roomsToGenerate);

        //Make and shuffle room ids

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
                corridorAttempt.SetupCorridor(possibleDoorway, board, 0, identifier);

                Room roomAttempt = RandomRoom();
                roomAttempt.SetupRoom(corridorAttempt.door2, identifier);

                bool valid = roomAttempt.TestRoomValidity(board);

                //Do testing
                if (valid)
                {
                    roomAttempt.PostValiditySetup();
                    corridorAttempt.door2.room = roomAttempt;
                    stemRoom.doorways.Add(possibleDoorway);
                    corridors.Add(corridorAttempt);
                    rooms.Add(roomAttempt);
                    roomRngIdentifiers.RemoveAt(0);
                }
                else
                {
                    i--;
                    failedRoomAttempts++;
                }
            }
            else
            {
                i--;
                failedRoomAttempts++;
            }
        }
    }

    public Doorway GenerateStartingDoorway(Board board)
    {
        int rng = UnityEngine.Random.Range(0, 4);
        switch (rng)
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

    private void InitRngIdentifiers(int roomsToGenerate)
    {
        //Go to roomsToGenerate - 1, because we already are making the starting room
        for (int i = 0; i < roomsToGenerate - 1; i++)
        {
            roomRngIdentifiers.Insert(UnityEngine.Random.Range(0, i), i);
        }
    }

    private Room SelectStemRoom()
    {
        return rooms[UnityEngine.Random.Range(0, rooms.Count)];
    }

    public void SetTilesValuesForRooms(Board board)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            try
            {
                rooms[i].PrintToTilesArray(board.tiles);
            }
            catch (System.Exception)
            {
                Debug.LogError("Failed tileprint of " + rooms[i].ToString());
            }
        }
    }
    public void SetTilesValuesForCorridors(Board board)
    {
        for (int i = 0; i < corridors.Count; i++)
        {
            try
            {
                corridors[i].PrintToTilesArray(board.tiles);
            }
            catch (System.Exception)
            {
                Debug.LogError("Failed tileprint of " + corridors[i].ToString());
            }
        }
    }

    public void PrintAllRooms()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            Debug.Log(rooms[i].ToString());
        }
    }


}
