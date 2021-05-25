using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGamePlayer : NetworkBehaviour
{

    public bool IsReady = false;

	private NetworkManagerGhostGame network;

	private NetworkManagerGhostGame Network
	{
		get
		{
			if (network != null) { return network; }
			return network = NetworkManager.singleton as NetworkManagerGhostGame;
		}
	}

	public PlayerCharacter localPlayer;


	public void Awake()
	{
		DontDestroyOnLoad(gameObject);
		Network.GamePlayers.Add(this);
	}

	[Command]
	public void CmdReadyUp()
	{
		IsReady = true;
		Network.NotifyGameOfReadyState();
	}

}
