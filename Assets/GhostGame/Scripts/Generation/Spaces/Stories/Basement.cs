using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basement : Story
{

    public Basement(int identifier) : base(identifier)
	{

	}

    protected override void GenerateLandingRoom(Board board, Vector2Int stairway, Verticality verticality)
    {
        landing = new BasementLanding();
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
                return new Catacomb();
            case 1:
                return new Catacomb();
            case 2:
                return new Catacomb();
            case 3:
                return new Catacomb();
            case 4:
                return new Catacomb();
            case 5:
                return new Catacomb();
            case 6:
                return new Cellar();
            case 7:
                return new BoilerRoom();
            case 8:
                return new PanicRoom();
            default:
                return new Room();
		}
	}
}
