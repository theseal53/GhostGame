using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catacomb : Room
{
	private CatacombTileset tileset;

	public Catacomb() : base()
	{
		roomCode = RoomCode.Catacomb;
		tileset = (CatacombTileset)TileSetRegistry.I.GetTileSet(roomCode);
	}
}
