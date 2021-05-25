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

	public List<Tilemap> floorTilemaps;
	public List<Tilemap> wallTilemaps;

	public Tile wall;

	[Header("Test")]
	[SerializeField] private TileSet Test1 = null;
	[SerializeField] private TileSet Test2 = null;
	[SerializeField] private TileSet Test3 = null;
	[SerializeField] private TileSet Test4 = null;
	[Header("Ground Story")]
	[SerializeField] private StartingRoomTileSet StartingRoom = null;
	[SerializeField] private LibraryTileset Library = null;
	[SerializeField] private DiningRoomTileset DiningRoom = null;
	[SerializeField] private KitchenTileset Kitchen = null;
	[SerializeField] private MasterBedroomTileset MasterBedroom = null;
	[SerializeField] private GameRoomTileset GameRoom = null;
	[SerializeField] private LoungeTileset Lounge = null;
	[Header("Basement")]
	[SerializeField] private BasementLandingTileset BasementLanding = null;
	[SerializeField] private CatacombTileset Catacomb = null;
	[SerializeField] private CellarTileset Cellar = null;
	[SerializeField] private BoilerRoomTileset BoilerRoom = null;
	[SerializeField] private PanicRoomTileset PanicRoom = null;
	[Header("Upper Story")]
	[SerializeField] private UpperStoryLandingTileset UpperStoryLanding = null;
	[SerializeField] private GuestBedroomTileset GuestBedroom = null;
	[SerializeField] private ServantBedroomTileset ServantBedroom = null;
	[SerializeField] private StudyTileset Study = null;
	//Shared
	[SerializeField] private BathroomTileset Bathroom = null;

	[SerializeField] private TileCollection wallTileCollection;


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

			//Ground story
			case RoomCode.StartingRoom:
				return StartingRoom;
			case RoomCode.Library:
				return Library;
			case RoomCode.DiningRoom:
				return DiningRoom;
			case RoomCode.Kitchen:
				return Kitchen;
			case RoomCode.MasterBedroom:
				return MasterBedroom;
			case RoomCode.GameRoom:
				return GameRoom;
			case RoomCode.Lounge:
				return Lounge;
			//Basement
			case RoomCode.BasementLanding:
				return BasementLanding;
			case RoomCode.Catacomb:
				return Catacomb;
			case RoomCode.Cellar:
				return Cellar;
			case RoomCode.BoilerRoom:
				return BoilerRoom;
			case RoomCode.PanicRoom:
				return PanicRoom;
			//Upper Story
			case RoomCode.UpperStoryLanding:
				return UpperStoryLanding;
			case RoomCode.GuestBedroom:
				return GuestBedroom;
			case RoomCode.ServantBedroom:
				return ServantBedroom;
			case RoomCode.Study:
				return Study;
			//Shared
			case RoomCode.Bathroom:
				return Bathroom;

			default:
				return Test1;
		}
	}

	public TileCollection GetWallTileCollection(int story)
	{
		return wallTileCollection;
	}
}
