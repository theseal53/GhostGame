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

    public bool exit;

    public Doorway(int x, int y, Direction roomOutDirection, bool exit = false)
	{
        this.x = x;
        this.y = y;
        this.roomOutDirection = roomOutDirection;
        this.exit = exit;
	}

    public Doorway(int x, int y, Direction roomOutDirection, Room room, Corridor corridor)
    {
        this.x = x;
        this.y = y;
        this.roomOutDirection = roomOutDirection;
        this.room = room;
        this.corridor = corridor;
    }
}
