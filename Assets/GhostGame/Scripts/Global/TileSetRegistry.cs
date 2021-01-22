using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetRegistry : MonoBehaviour
{

	private void Awake()
	{
		i = this;
	}

	private static TileSetRegistry i;

    public static TileSetRegistry I
	{
        get
		{
			return i;
		}
		set
		{
			i = value;
		}
	}

	public Tile wall;
	public Tilemap floorTilemap;
	public Tilemap wallTilemap;


	[SerializeField] private TileSet Test1;
	[SerializeField] private TileSet Test2;
	[SerializeField] private TileSet Test3;
	[SerializeField] private TileSet Test4;

	[SerializeField] private StartingRoomTileSet StartingRoom;
	[SerializeField] private LibraryTileset Library;
	[SerializeField] private DiningRoomTileset DiningRoom;
	[SerializeField] private BathroomTileset Bathroom;
	[SerializeField] private KitchenTileset Kitchen;

	public TileSet GetTileSet(RoomCode code)
	{
		switch (code)
		{
			case RoomCode.Test1:
				return Test1;
			case RoomCode.Test2:
				return Test2;
			case RoomCode.Test3:
				return Test3;
			case RoomCode.Test4:
				return Test4;

			case RoomCode.StartingRoom:
				return StartingRoom;
			case RoomCode.Library:
				return Library;
			case RoomCode.DiningRoom:
				return DiningRoom;
			case RoomCode.Bathroom:
				return Bathroom;
			case RoomCode.Kitchen:
				return Kitchen;

			default:
				return Test1;
		}
	}
}
