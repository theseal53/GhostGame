using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundStory : Story
{
    public GroundStory(int identifier) : base(identifier)
    {

    }

    public virtual void CreateRoomsAndCorridors(Board board)
    {
        GenerateLandingRoom(board);
        GenerateRemainingRooms(board);
    }

    protected void GenerateLandingRoom(Board board)
    {
        StartingRoom startingRoom = new StartingRoom();
        landing = startingRoom;
        rooms.Add(startingRoom);
        Doorway startingDoorway = GenerateStartingDoorway(board);
        startingRoom.SetupRoom(startingDoorway, identifier);
        startingRoom.TestRoomValidity(board);
        board.startingRoom = startingRoom;
    }

	protected override Room RandomRoom()
	{
        int rng = roomRngIdentifiers[0];
        switch (rng)
        {
            case 0:
                return new Kitchen();
            case 1:
                return new Library();
            case 2:
                return new MasterBedroom();
            case 3:
                return new Bathroom();
            case 4:
                return new Bathroom();
            case 5:
                return new GameRoom();
            case 6:
                return new Lounge();
            case 7:
                return new Lounge();
            case 8:
                return new DiningRoom();
            default:
                return new Room();
        }
    }
}
