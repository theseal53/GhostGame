using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestBedroom : SmallBedroom
{
	private GuestBedroomTileset tileset;

	public GuestBedroom() : base()
	{
		widthRange = new IntRange(6, 9);
		heightRange = new IntRange(6, 9);
		roomCode = RoomCode.GuestBedroom;
		tileset = (GuestBedroomTileset)TileSetRegistry.I.GetTileSet(roomCode);
	}

	public override void GenerateFurniture()
	{
		GenerateLightSwitch();
		GenerateCornerBed(tileset.bed);
	}
}
