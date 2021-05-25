using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabRegistry : MonoBehaviour
{
	private void Awake()
	{
		i = this;
	}

	private static PrefabRegistry i;

	public static PrefabRegistry I
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

	public List<GameObject> furnitureContainers;
	public List<GameObject> mobsContainers;
	public List<GameObject> itemsContainers;

	public GameObject player;
	public GameObject ghost;
	public GameObject crucifix;
	public GameObject electricLight;
	public GameObject boxOverlay;
	public GameObject lightSwitch;
	public GameObject standardWallLight;
	public GameObject spiderChandelier;
	public GameObject ceilingChainLight;
}
