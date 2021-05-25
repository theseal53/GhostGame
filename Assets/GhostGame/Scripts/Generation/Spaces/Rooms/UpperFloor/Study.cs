using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Study : Room
{

    private TileSet tileSet;

    public Study() : base()
	{
		roomCode = RoomCode.Study;
		tileSet = TileSetRegistry.I.GetTileSet(roomCode);
	}
}
