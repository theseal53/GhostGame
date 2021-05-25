using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.Tilemaps;

public class GameManager : NetworkBehaviour
{
    private static GameManager i;

    public static GameManager I
	{
        get
		{
            return i;
		}
	}

    public Board board;

    [HideInInspector]
    public List<PlayerCharacter> players = new List<PlayerCharacter>();

    [HideInInspector]
    public PlayerCharacter localPlayer;

    [HideInInspector]
    public CameraFollow mainCameraFollow;

    public TimerManager gameTimerManager;
    public Timer gameTimer;

    public ActiveGamePlayer activeGamePlayer;
    public PlayerCharacter playerCharacterPrefab;

    [SyncVar]
    int seed;


    private NetworkManagerGhostGame network;
    private NetworkManagerGhostGame Network
    {
        get
        {
            if (network != null) { return network; }
            return network = NetworkManager.singleton as NetworkManagerGhostGame;
        }
    }



    public GameManager() : base()
	{
        i = this;
	}

	public override void OnStartServer()
	{
        seed = System.DateTime.Now.Millisecond;
	}

	public override void OnStartClient()
	{
        foreach(ActiveGamePlayer player in Network.GamePlayers)
		{
            if (player.isLocalPlayer)
			{
                activeGamePlayer = player;
			}
		}
        GenerateBoard();
        activeGamePlayer.CmdReadyUp();
	}

    public void GenerateBoard()
    {
        UnityEngine.Random.InitState(seed);
        BoardDataGenerator boardDataGenerator = new BoardDataGenerator();
        board = boardDataGenerator.GenerateBoardData(Constants.NUM_STORIES, Constants.NUM_COLUMNS, Constants.NUM_ROWS);
        SpawnTiles(board.tiles);
        SpawnObjects();
    }

    [Client]
    public void SpawnTiles(sbyte[][][] tiles)
    {
        TilePositionParser tilePositionParser = new TilePositionParser();
        TilePosition[][][] tilePositions = tilePositionParser.Parse(tiles);
        TileInstantiator tileInstantiator = new TileInstantiator();
        tileInstantiator.InstantiateTiles(tiles, tilePositions);
    }

    public void SpawnObjects()
    {
        BoardPopulator boardPopulator = new BoardPopulator();
        boardPopulator.PopulateBoard(board);

        GhostGenerator ghostGenerator = new GhostGenerator();
        ghostGenerator.GenerateGhost(board);
    }

    [Server]
    public void BeginGame()
	{
		for (int i = 0; i < Network.GamePlayers.Count; i++)
		{
            ActiveGamePlayer player = Network.GamePlayers[i];
            PlayerCharacter playerCharacter = Instantiate(playerCharacterPrefab);
            playerCharacter.transform.position = board.startingRoom.StartingPositions()[i];
            playerCharacter.storyLocation = Constants.GROUND_STORY;
            NetworkServer.Spawn(playerCharacter.gameObject, player.connectionToClient);
            TargetSetCameraTarget(player.connectionToClient, playerCharacter.gameObject);
		}
        gameTimerManager = gameObject.AddComponent<TimerManager>();
        gameTimer = new Timer(60, gameTimerManager);
        gameTimer.Start();
    }

    [TargetRpc]
    public void TargetSetCameraTarget(NetworkConnection conn, GameObject target)
    {
        mainCameraFollow = Camera.main.GetComponent<CameraFollow>();
        mainCameraFollow.SetTarget(target.GetComponent<Entity>());
    }

    [Server]
    private void FixedUpdate()
	{
        EventHub.GameTimeChangeBroadcast(gameTimer.TimeRemaining);
	}
}
