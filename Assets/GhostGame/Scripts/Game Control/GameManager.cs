using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
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
    TileInstantiator tileInstantiator;
    BoardPopulator boardPopulator;
    GhostGenerator ghostGenerator;

    Board board;

    public GameObject playerPrefab;
    public GameObject player;

    public CameraFollow mainCameraFollow;

    public TimerManager gameTimerManager;
    public Timer gameTimer;

    // Start is called before the first frame update
    void Start()
    {
        i = this;

        gameTimerManager = gameObject.AddComponent<TimerManager>();
        gameTimer = new Timer(60, gameTimerManager);
        gameTimer.Start();

        boardDataGenerator = new BoardDataGenerator();
        board = boardDataGenerator.GenerateBoardData();

        tileInstantiator = new TileInstantiator();
        tileInstantiator.InstantiateTiles(board);

        boardPopulator = new BoardPopulator();
        boardPopulator.PopulateBoard(board);

        player = Instantiate(playerPrefab);
        player.transform.position = board.startingRoom.StartingPositions()[0];

        ghostGenerator = new GhostGenerator();
        ghostGenerator.GenerateGhost(board);

        mainCameraFollow.target = player;

        //ArrayPrinter.Print2DArray(board.tiles, "Assets/test.txt");
    }

	private void Update()
	{
        //TileSetRegistry.I.wallTilemap.GetComponent<SetTilemapShadows>().AddShadows();
    }

    private void FixedUpdate()
	{
        EventHub.GameTimeChangeBroadcast(gameTimer.TimeRemaining);
	}
}
