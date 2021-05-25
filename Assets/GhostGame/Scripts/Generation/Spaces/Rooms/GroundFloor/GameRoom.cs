using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoom : Room
{
	private GameRoomTileset tileset;

	public GameRoom() : base()
	{
		roomCode = RoomCode.GameRoom;
		tileset = (GameRoomTileset)TileSetRegistry.I.GetTileSet(roomCode);
	}
}
