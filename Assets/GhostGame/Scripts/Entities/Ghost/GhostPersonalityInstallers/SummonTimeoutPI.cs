using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class SummonTimeoutPI : GhostPersonalityInstaller
{
	float timeRemainingForSummon = 58;
	public override bool InstallPersonality(Ghost ghost)
	{
		if (ghost.summonTransitionConditions.Count == 0)
		{
			ghost.summonTransitionConditions.Add(new TimeoutTC(ghost, ghost.SummonTransitionState, timeRemainingForSummon));
			return true;
		}
		return false;
	}
}
