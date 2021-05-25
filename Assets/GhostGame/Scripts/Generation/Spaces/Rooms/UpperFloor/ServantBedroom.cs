using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServantBedroom : SmallBedroom
{
	private ServantBedroomTileset tileset;

	public ServantBedroom() : base()
	{
		widthRange = new IntRange(6, 9);
		heightRange = new IntRange(6, 9);
		roomCode = RoomCode.ServantBedroom;
		tileset = (ServantBedroomTileset)TileSetRegistry.I.GetTileSet(roomCode);
	}

	public override void GenerateFurniture()
	{
		GenerateLightSwitch();
		GenerateCornerBed(tileset.bed);
	}
}
