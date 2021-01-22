using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Mirror;
using System;

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

    BoardDataGenerator boardDataGenerator;
    TilePositionParser tilePositionParser;
    TileInstantiator tileInstantiator;
    BoardPopulator boardPopulator;
    GhostGenerator ghostGenerator;

    public Board board;
    public int columns = 100;
    public int rows = 100;

    public List<PlayerCharacter> players = new List<PlayerCharacter>();

    public CameraFollow mainCameraFollow;

    public TimerManager gameTimerManager;
    public Timer gameTimer;

    public List<GameObject> clientOnlyObjects;


    public GameManager() : base()
	{
        i = this;
	}

    //Server do this
	public void Awake()
    {
        //gameTimerManager = gameObject.AddComponent<TimerManager>();
        //gameTimer = new Timer(60, gameTimerManager);
        //gameTimer.Start();
    }

    public void GenerateBoard()
    {
        print("GenerateBoard");
        boardDataGenerator = new BoardDataGenerator();
        board = boardDataGenerator.GenerateBoardData(columns, rows);

        boardPopulator = new BoardPopulator();
        boardPopulator.PopulateBoard(board);

        ghostGenerator = new GhostGenerator();
        ghostGenerator.GenerateGhost(board);
    }




    /*public void RpcSpawnTiles(short[] rawTiles)
    {
        Debug.Log("Entered spawn tiles");
        short[,] tiles = ArrayPrinter.To2DArray(rawTiles, rows, columns);
        tilePositionParser = new TilePositionParser();
        TilePosition[,] tilePositions = tilePositionParser.Parse(tiles);

        tileInstantiator = new TileInstantiator();
        tileInstantiator.InstantiateTiles(tiles, tilePositions);
    }*/

    /*public override void OnStartClient()
	{
		base.OnStartClient();
        foreach (GameObject target in clientOnlyObjects)
        {
            target.SetActive(true);
        }
    }

    public void AddPlayer(PlayerCharacter player)
	{
        players.Add(player);
	}

    public void BeginPlay()
	{
        print("Begin play in GameManager");
        foreach(PlayerCharacter player in players)
		{
            player.BeginPlay();
		}
	}*/

    /*private void FixedUpdate()
	{
        EventHub.GameTimeChangeBroadcast(gameTimer.TimeRemaining);
	}*/
}
