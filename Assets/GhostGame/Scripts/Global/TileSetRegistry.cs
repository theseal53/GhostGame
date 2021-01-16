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

	public TileSet Test1;
	public TileSet Test2;
	public TileSet Test3;
	public TileSet Test4;

	public StartingRoomTileSet StartingRoom;
	public LibraryTileset Library;
	public DiningRoomTileset DiningRoom;
	public BathroomTileset Bathroom;
}
