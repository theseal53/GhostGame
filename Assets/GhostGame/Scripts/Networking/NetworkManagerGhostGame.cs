using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class NetworkManagerGhostGame : NetworkManager
{

    [SerializeField] private int minPlayers = 1;
    [Scene] [SerializeField] private string menuScene = string.Empty;
    [Scene] [SerializeField] private string playScene = string.Empty;

    [Header("Room")]
    [SerializeField] private LobbyPlayer LobbyPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private ActiveGamePlayer ActiveGamePlayerPrefab = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action OnServerReadied;

    public List<LobbyPlayer> LobbyPlayers { get; } = new List<LobbyPlayer>();
    public List<ActiveGamePlayer> GamePlayers { get; } = new List<ActiveGamePlayer>();
    public List<NetworkConnection> Connections { get; } = new List<NetworkConnection>();


    //Generic

	public override void OnClientConnect(NetworkConnection conn)
	{
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
	}

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }

	public override void OnServerConnect(NetworkConnection conn)
	{
	    if (numPlayers >= maxConnections)
		{
            conn.Disconnect();
            return;
		}
        if (SceneManager.GetActiveScene().path != menuScene)
		{
            OnServerConnectLobby(conn);
        }
	}

    public override void OnServerAddPlayer(NetworkConnection conn)
	{
        if (SceneManager.GetActiveScene().path == menuScene)
		{
            OnServerAddPlayerLobby(conn);
		}
	}

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            OnServerDisconnectLobby(conn);
        }
		base.OnServerDisconnect(conn);
	}

    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            ServerChangeSceneLobby(newSceneName);
        }
    }
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        if (SceneManager.GetActiveScene().path == menuScene)
        {
            OnServerReadyLobby();
        }
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().path == playScene)
        {
            OnClientSceneChangedGame(conn);
        }
    }

	public override void OnServerSceneChanged(string sceneName)
	{
        if (SceneManager.GetActiveScene().path == playScene)
		{
            OnServerSceneChangedGame();
		}
		base.OnServerSceneChanged(sceneName);
	}


	public override void OnStopServer()
	{
        LobbyPlayers.Clear();
        GamePlayers.Clear();
	}

    //Lobby////////////////////////////////////////////////////////////////////////

    private void OnServerConnectLobby(NetworkConnection conn)
    {
        conn.Disconnect();
        return;
    }

    private void OnServerAddPlayerLobby(NetworkConnection conn)
    {
        bool isLeader = LobbyPlayers.Count == 0;
        LobbyPlayer roomPlayerInstance = Instantiate(LobbyPlayerPrefab);
        Connections.Add(conn);
        roomPlayerInstance.IsLeader = isLeader;
        NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
    }

    private void OnServerDisconnectLobby(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            LobbyPlayer player = conn.identity.GetComponent<LobbyPlayer>();
            LobbyPlayers.Remove(player);
            NotifyLobbyOfReadyState();
        }
    }

    public void NotifyLobbyOfReadyState()
    {
        foreach (LobbyPlayer player in LobbyPlayers)
        {
            player.HandleReadyToStart(IsLobbyReadyToStart());
        }
    }

    public void TransitionLobbyToGame()
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            if (!IsLobbyReadyToStart()) { return; }
            ServerChangeScene(playScene);
        }
    }

    private void OnServerReadyLobby()
	{
        if (!IsLobbyReadyToStart())
        {
            return;
        }
        OnServerReadied?.Invoke();
    }

    private bool IsLobbyReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }
        foreach (LobbyPlayer player in LobbyPlayers)
        {
            if (!player.IsReady) { return false; }
        }
        return true;
    }

    void ServerChangeSceneLobby(string newSceneName)
	{
        for (int i = Connections.Count - 1; i >= 0; i--)
        {
            NetworkServer.Destroy(Connections[i].identity.gameObject);
        }
        base.ServerChangeScene(newSceneName);
    }

    //Active Game

    public void NotifyGameOfReadyState()
    {
        foreach (ActiveGamePlayer player in GamePlayers)
        {
            player.HandleReadyToStart(IsGameReadyToStart());
        }
    }

    bool IsGameReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }
        foreach (LobbyPlayer player in LobbyPlayers)
        {
            if (!player.IsReady) { return false; }
        }
        print("Game is ready to start!");
        return true;
    }

    void OnClientSceneChangedGame(NetworkConnection conn)
	{
        print("On Client scene change");

        print(GamePlayers.Count);
        /*foreach (ActiveGamePlayer player in GamePlayers)
        {
            if (player.isLocalPlayer)
			{
                player.RequestBoardInfo();
			}
        }*/
    }

    void OnServerSceneChangedGame()
	{
        print("Server scene changed");
        for (int i = 0; i < Connections.Count; i++)
        {
            NetworkConnection conn = Connections[i];
            ActiveGamePlayer activeGamePlayer = Instantiate(ActiveGamePlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, activeGamePlayer.gameObject);
        }
    }

    //Nothing yet
}
