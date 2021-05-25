using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperStory : Story
{
    public UpperStory(int identifier) : base(identifier)
    {

    }
    protected override void GenerateLandingRoom(Board board, Vector2Int stairway, Verticality verticality)
    {
        landing = new UpperStoryLanding();
        rooms.Add(landing);
        landing.SetupRoom(stairway, verticality, board.startingDoorwayDirection, identifier);
        landing.TestRoomValidity(board);
    }

    protected override Room RandomRoom()
    {
        int rng = roomRngIdentifiers[0];
        switch (rng)
        {
            case 0:
                return new GuestBedroom();
            case 1:
                return new GuestBedroom();
            case 2:
                return new GuestBedroom();
            case 3:
                return new Bathroom();
            case 4:
                return new Bathroom();
            case 5:
                return new ServantBedroom();
            case 6:
                return new ServantBedroom();
            case 7:
                return new ServantBedroom();
            case 8:
                return new Study();
            default:
                return new Room();
        }
    }
}
