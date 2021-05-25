using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPopulator
{
    public void PopulateBoard(Board board)
	{
		foreach (Story story in board.stories)
		{
			foreach (Room room in story.rooms)
			{
				room.GenerateFurniture();
				room.GenerateLights();
			}

		}

	}
}
