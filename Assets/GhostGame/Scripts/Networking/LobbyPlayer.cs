using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : NetworkBehaviour
{
	[Header("UI")]
	[SerializeField] private GameObject lobbyUI = null;
	[SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
	[SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
	[SerializeField] private Button startGameButton = null;

	[SyncVar(hook = nameof(HandleDisplayNameChanged))]
	public string DisplayName = "Loading...";
	[SyncVar(hook = nameof(HandleReadyStatusChanged))]
	public bool IsReady = false;

	private bool isLeader;

	public bool IsLeader
	{
		set
		{
			isLeader = value;
			startGameButton.gameObject.SetActive(value);
		}
	}
	private NetworkManagerGhostGame network;
	private NetworkManagerGhostGame Network
	{
		get
		{
			if (network != null) { return network; }
			return network = NetworkManager.singleton as NetworkManagerGhostGame;
		}
	}

	public override void OnStartAuthority()
	{
		CmdSetDisplayName(PlayerNameInput.DisplayName);
		lobbyUI.SetActive(true);
	}

	public override void OnStartClient()
	{
		Network.LobbyPlayers.Add(this);
		if (isLocalPlayer)
		{
			CmdReadyUp();
		}
		UpdateDisplay();
	}

	public override void OnStopClient()
	{
		Network.LobbyPlayers.Remove(this);
		UpdateDisplay();
	}

	public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
	public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

	private void UpdateDisplay()
	{
		if (!isLocalPlayer)
		{
			foreach(var player in Network.LobbyPlayers)
			{
				if (player.isLocalPlayer)
				{
					player.UpdateDisplay();
					break;
				}
			}
			return;
		}

		for (int i = 0; i < playerNameTexts.Length; i++)
		{
			playerNameTexts[i].text = "Waiting For Player...";
			playerReadyTexts[i].text = string.Empty;
		}

		for (int i = 0; i < Network.LobbyPlayers.Count; i++)
		{
			playerNameTexts[i].text = Network.LobbyPlayers[i].DisplayName;
			playerReadyTexts[i].text = Network.LobbyPlayers[i].IsReady ?
				"<color=green>Ready</color>" :
				"<color=red>Not Ready</color>";
		}

	}
	public void HandleReadyToStart(bool readyToStart)
	{
		if (!isLeader) { return; }
		startGameButton.interactable = readyToStart;
	}

	[Command]
	private void CmdSetDisplayName(string displayName)
	{
		DisplayName = displayName;
	}

	[Command]
	public void CmdReadyUp()
	{
		IsReady = !IsReady;
		network.NotifyLobbyOfReadyState();
	}

	[Command]
	public void CmdStartGame()
	{
		if (Network.LobbyPlayers[0].connectionToClient != connectionToClient) { return; }
		Network.TransitionLobbyToGame();
	}
}
