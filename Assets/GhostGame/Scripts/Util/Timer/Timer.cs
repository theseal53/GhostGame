using System.Collections.Generic;

public class Timer
{
    public float timePassed = 0;
    public float TimeRemaining
	{
        get
		{
            return expireTime - timePassed;
		}
	}
    public float expireTime;
    public bool isComplete = false;
    public delegate void TimeoutFunction();
    public TimerManager cluster;
    private HashSet<TimeoutFunction> timeoutFunctions;

    public Timer(float expireTime, TimerManager cluster)
    {
        this.expireTime = expireTime;
        this.cluster = cluster;
        timeoutFunctions = new HashSet<TimeoutFunction>();
    }
    public void AddTimeoutFunction(TimeoutFunction timeoutFunction)
    {
        timeoutFunctions.Add(timeoutFunction);
    }
    public void RemoveTimeoutFunction(TimeoutFunction timeoutFunction)
    {
        //Potentially risky code- what happens if it's not there?
        timeoutFunctions.Remove(timeoutFunction);
    }
    public void ClearTimeoutFunctions()
	{
        timeoutFunctions.Clear();
	}
    public void ExecuteTimeoutFunctions()
    {
        foreach (TimeoutFunction timeoutFunction in timeoutFunctions)
        {
            timeoutFunction();
        }
    }
    public void Start()
	{
        cluster.StartTimer(this);
	}
    public void Stop()
	{
        cluster.StopTimer(this);
	}
    public void Reset()
	{
        cluster.ResetTimer(this);
	}
}
