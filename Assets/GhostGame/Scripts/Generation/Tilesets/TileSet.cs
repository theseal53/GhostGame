using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet : MonoBehaviour
{
	[SerializeField]
	private TileCollection[] tileCollections;

	private TileCollection tileCollection;
	public TileCollection TileCollection
	{
		get
		{
			if (tileCollection == null)
			{
				if (tileCollections.Length == 0)
				{
					throw new System.Exception("Tileset has not been given a TileCollection in the Inspector");
				}
				tileCollection = tileCollections[Random.Range(0, tileCollections.Length)];
			}
			return tileCollection;
		}
	}
}
