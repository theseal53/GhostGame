using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPopulator
{
    public void PopulateBoard(Board board)
	{
		foreach(Room room in board.rooms)
		{
			room.GenerateFurniture();
			room.GenerateLights();
		}
	}
}
