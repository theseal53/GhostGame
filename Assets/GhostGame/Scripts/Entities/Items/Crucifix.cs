using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crucifix : Item
{

	public float banishRadius = 2;

    public override void UseByCharacter()
	{
		print("Be banished!!");
		Collider2D[] proximityObjects = Physics2D.OverlapCircleAll(transform.position, banishRadius);
		foreach (Collider2D collider in proximityObjects)
		{
			Ghost ghost = collider.GetComponent<Ghost>();
			if (ghost != null)
			{
				ghost.Banish();
			}
		}
	}
}
