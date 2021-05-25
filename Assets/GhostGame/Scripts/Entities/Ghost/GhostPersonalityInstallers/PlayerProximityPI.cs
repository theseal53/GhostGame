using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class PlayerProximityPI : GhostPersonalityInstaller
{
	public override bool InstallPersonality(Ghost ghost)
	{
		if (ghost.aggroTransitionConditions.Count == 0 && ghost.passiveTransitionConditions.Count == 0)
		{
			ghost.passiveTransitionConditions.Add(new PlayerInsideProximityTC(ghost, ghost.AggroState));
			ghost.aggroTransitionConditions.Add(new PlayerOutsideProximityTC(ghost, ghost.PassiveState));
			return true;
		}
		return false;
	}
}
