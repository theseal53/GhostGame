using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway
{
    public int x;
    public int y;
    public Direction roomOutDirection;

    public Room room;
    public Corridor corridor;

    //Doorways will always expand in the positive X or Y directions
    public int breadth;

    public bool isMapExit;

    public Doorway(int x, int y, Direction roomOutDirection, int breadth = Constants.DEFAULT_DOOR_BREADTH, bool isMapExit = false)
	{
        this.x = x;
        this.y = y;
        this.roomOutDirection = roomOutDirection;
        this.breadth = breadth;
        this.isMapExit = isMapExit;
	}

    public Doorway(int x, int y, Direction roomOutDirection, Room room, Corridor corridor, int breadth = Constants.DEFAULT_DOOR_BREADTH)
    {
        this.x = x;
        this.y = y;
        this.roomOutDirection = roomOutDirection;
        this.breadth = breadth;
        this.room = room;
        this.corridor = corridor;
    }
}
