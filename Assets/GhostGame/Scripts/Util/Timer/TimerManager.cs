using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TimerManager is responsible for actually keeping track of the time. Each Timer must have a TimerManager
//TimerManager is meant to be added as a component on a gameobject. This enables it to have access to FixedUpdate()
public class TimerManager : MonoBehaviour
{

    public bool isPaused = false;
    //There will be no duplicates
    private HashSet<Timer> myTimers;

    // Use this for initialization
    void Awake()
    {
        myTimers = new HashSet<Timer>();
    }

    // Timer will be stopped once it has completed
    void FixedUpdate()
    {
        if (!isPaused)
        {
            List<Timer> timersCopy = myTimers.ToList<Timer>(); //Creates a shallow copy so we can edit original
            foreach (Timer targetTimer in timersCopy)
            {
                targetTimer.timePassed += Time.deltaTime;
                if (targetTimer.timePassed >= targetTimer.expireTime)
                {
                    targetTimer.isComplete = true;
                    myTimers.Remove(targetTimer);
                    targetTimer.ExecuteTimeoutFunctions();
                }
            }
        }
    }
    ///If timer has finished, it will be reset and started again. Otherwise it will simply resume
    ///Starting a timer while it is in progress will not cause any change
    public void StartTimer(Timer xoloTimer)
    {
        if (xoloTimer.isComplete)
        {
            xoloTimer.timePassed = 0;
            xoloTimer.isComplete = false;
        }
        myTimers.Add(xoloTimer);
    }
    ///Stopping timer will not reset its current count
    public void StopTimer(Timer xoloTimer)
    {
        myTimers.Remove(xoloTimer);
    }
    ///Reseting timer will stop the timer and reset its current count
    public void ResetTimer(Timer xoloTimer)
    {
        myTimers.Remove(xoloTimer);
        xoloTimer.timePassed = 0;
        xoloTimer.isComplete = false;
    }
}