using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class AppeaseTimeoutPI : GhostPersonalityInstaller
{
	float timeRemainingForAppeasement = 55;
	public override bool InstallPersonality(Ghost ghost)
	{
		if (ghost.appeaseConditionInstalled == false)
		{
			ghost.generalTransitionConditions.Add(new TimeoutTC(ghost, ghost.AppeasedTransitionState, timeRemainingForAppeasement));
			ghost.appeaseConditionInstalled = true;
			return true;
		}
		return false;
	}
}
