using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : Entity
{
    public bool spawnOnServer = false;

	protected override void SortIntoParent()
	{
		transform.SetParent(PrefabRegistry.I.furnitureContainers[storyLocation].transform);
	}
}
