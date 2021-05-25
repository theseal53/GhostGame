using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class BanishTimeoutPI : GhostPersonalityInstaller
{
	float timeRemainingForBanishment = 54;
	public override bool InstallPersonality(Ghost ghost)
	{
		if (ghost.banishConditionInstalled == false)
		{
			ghost.generalTransitionConditions.Add(new TimeoutTC(ghost, ghost.BanishedTransitionState, timeRemainingForBanishment));
			ghost.banishConditionInstalled = true;
			return true;
		}
		return false;
	}
}
