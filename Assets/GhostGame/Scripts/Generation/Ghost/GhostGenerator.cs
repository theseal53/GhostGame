using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGenerator
{

	private List<GhostPersonalityInstaller> personalityInstallers = new List<GhostPersonalityInstaller>()
	{
			new PlayerProximityPI(),
			new SummonTimeoutPI(),
			new AppeaseTimeoutPI(),
			new BanishTimeoutPI()
	};
	private List<float> PIChances = new List<float>()
	{
		.1f,
		.1f,
		.1f,
		.1f
	};

	public Ghost GenerateGhost(Board board)
	{
		Ghost ghost = Object.Instantiate(PrefabRegistry.I.ghost).GetComponent<Ghost>();
		ghost.Init(0);
		ghost.nameSet = RandomName.Generate();

		ghost.summonRoom = RandomRoom(board);
		ghost.summonPosition = FindSummonPosition(ghost.summonRoom);

		InstallRandomPersonality(ghost);

		return ghost;
	}

	private void InstallRandomPersonality(Ghost ghost)
	{
		int maxAttempts = 100;
		int currentAttempts = 0;
		while (!ghost.PersonalityIsComplete())
		{
			if (personalityInstallers.Count == 0)
			{
				throw new System.Exception("Ran out of GhostPersonalityInstallers");
			}
			GhostPersonalityInstaller personalityInstaller = WeightedChoice.Choose(personalityInstallers, PIChances);
			personalityInstaller.InstallPersonality(ghost);
			if (maxAttempts <= currentAttempts)
			{
				throw new System.Exception("Installing personality took too long");
			}
			++currentAttempts;
		}

	}

	/*private float RandomSpeed()
	{
		float slowSpeed = 1;
		float mediumSpeed = 3;
		float fastSpeed = 10;
		int rng = Random.Range(0, 3);
		switch(rng)
		{
			case 0:
				return slowSpeed;
			case 1:
				return mediumSpeed;
			default:
				return fastSpeed;
		}
	}*/
	private Room RandomRoom(Board board)
	{
		int rng = Random.Range(0, board.stories[0].rooms.Count);
		return board.stories[0].rooms[rng];
	}
	private Vector2 FindSummonPosition(Room room)
	{
		return new Vector2(room.x + room.width / 2, room.y + room.height / 2);
	}
}
