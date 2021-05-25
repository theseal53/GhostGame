using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStart : MonoBehaviour
{

	[SerializeField] private NetworkManagerGhostGame networkManager = null;
	public void StartGame()
	{

		networkManager.quickStart = true;
		networkManager.StartHost();
	}
}
