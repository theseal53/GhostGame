using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static XoloStateMachine;

public abstract class GhostPersonalityInstaller
{
	/// <summary>
	/// Returns true if anything was modified
	/// </summary>
	/// <returns></returns>
	public abstract bool InstallPersonality(Ghost ghost);
}
