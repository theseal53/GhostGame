using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// The center of all stimuli in the game. Whenever a stimulus occurs, it should go through this 
/// </summary>
class EventHub
{

    public delegate void GameTimeChangeDelegate(float time);
    public static event GameTimeChangeDelegate GameTimeChange;
    public static void GameTimeChangeBroadcast(float time)
    {
        GameTimeChange?.Invoke(time);
    }

    public delegate void PlayerChangeStoryDelegate(PlayerCharacter playerCharacter);
    public static event PlayerChangeStoryDelegate PlayerChangeStory;
    public static void PlayerChangeStoryBroadcast(PlayerCharacter playerCharacter)
    {
        PlayerChangeStory?.Invoke(playerCharacter);
    }



}
