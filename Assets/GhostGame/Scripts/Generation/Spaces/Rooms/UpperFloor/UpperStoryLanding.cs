using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperStoryLanding : Landing
{
	private BasementLandingTileset tileset;

	public UpperStoryLanding() : base()
	{
		roomCode = RoomCode.BasementLanding;
		tileset = (BasementLandingTileset)TileSetRegistry.I.GetTileSet(roomCode);
	}

	public override void GenerateFurniture()
	{
		GenerateLightSwitch();
		//InstantiateFurniture(tileset.staircaseUp, stairwayUp);
		InstantiateFurniture(tileset.staircaseDown, stairwayDown);
	}
}
