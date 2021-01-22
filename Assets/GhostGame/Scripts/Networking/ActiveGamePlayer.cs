using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGamePlayer : NetworkBehaviour
{

    public bool IsReady = false;

	private NetworkManagerGhostGame network;

	public GameManager gameManager;

	private NetworkManagerGhostGame Network
	{
		get
		{
			if (network != null) { return network; }
			return network = NetworkManager.singleton as NetworkManagerGhostGame;
		}
	}

	public override void OnStartClient()
	{
		Network.GamePlayers.Add(this);
		print("On start client");
		if (isLocalPlayer)
		{
			RequestBoardInfo();
		}
	}

	public override void OnStartServer()
	{
		Network.GamePlayers.Add(this);
		gameManager = Instantiate(gameManager);
	}

	public override void OnStopClient()
	{
		Network.GamePlayers.Remove(this);
	}

	[Command]
	public void CmdReadyUp()
	{
		IsReady = !IsReady;
		Network.NotifyGameOfReadyState();
	}

	public void HandleReadyToStart(bool readyToStart)
	{
		//if (!isLeader) { return; }
		//startGameButton.interactable = readyToStart;
	}

	public void RequestBoardInfo()
	{
		print("request board info");
		CmdSendBoardInfo();
	}

	[Command]
	public void CmdSendBoardInfo()
	{
		print("CmdSendBoardInfo");
		gameManager.GenerateBoard();
		RpcSpawnTiles(ArrayPrinter.To1DArray(gameManager.board.tiles), gameManager.rows, gameManager.columns);
		//RpcSpawnTiles(new short[] { 1, 2, 3 }, 10, 10);
	}

	[ClientRpc(channel=2)]
	public void RpcSpawnTiles(short[] rawTiles, int rows, int columns)
	{
		print("Rpc spawn tiles");
		short[,] tiles = ArrayPrinter.To2DArray(rawTiles, rows, columns);
		TilePositionParser tilePositionParser = new TilePositionParser();
		TilePosition[,] tilePositions = tilePositionParser.Parse(tiles);
		TileInstantiator tileInstantiator = new TileInstantiator();
		tileInstantiator.InstantiateTiles(tiles, tilePositions);
	}


}
