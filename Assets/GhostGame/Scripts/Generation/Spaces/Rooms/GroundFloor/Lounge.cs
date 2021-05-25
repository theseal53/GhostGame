using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lounge : Room
{
	private LoungeTileset tileset;

	public Lounge() : base()
	{
		roomCode = RoomCode.Lounge;
		tileset = (LoungeTileset)TileSetRegistry.I.GetTileSet(roomCode);
	}
}
