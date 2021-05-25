using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBedroom : Room
{
	private MasterBedroomTileset tileset;

	public MasterBedroom() : base()
	{
		roomCode = RoomCode.MasterBedroom;
		tileset = (MasterBedroomTileset)TileSetRegistry.I.GetTileSet(roomCode);
	}
}
