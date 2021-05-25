//States are stored on a stack. The only state that you are able to view is on the very top.
//A state function is a function in another class that is called on a regular basis and defines that class' behavior at the time
    //State functions should be responsible for transitioning from state to state.
    //Therefore, the only functions that should call PushState() and PopState() are state functions
//If an exteral factor (such as the click of a button) wishes to cause a state change, it should use the requestedStates queue
    //This allows the current state to correctly deal with the state transition

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XoloStateMachine
{
    public delegate void State();
    private Stack<State> stateList;
    public bool stateIsNew;
    private bool statePersists;
    private Queue<State> requestedStates;

    public XoloStateMachine()
    {
        stateList = new Stack<State>();
        requestedStates = new Queue<State>();
    }
    //Simply returns the current state
    public State CurrentState
    {
        get
        {
			if (stateList.Count > 0) {
				return (stateList.Peek());
			} else {
				return null;
			}
        }
    }
    public void ExecuteState()
    {
        statePersists = true;
        if (stateList.Peek() != null)
        {
            stateList.Peek()(); //See the top state and call the function
            if (statePersists)
            {
                stateIsNew = false;
            }
        }
    }
    public void PushState(State newState)
    {
        stateList.Push(newState);
        stateIsNew = true;
        statePersists = false;
    }
    public void PopState()
    {
        stateList.Pop();
        stateIsNew = true;
        statePersists = false;
    }
    public void Reset()
    {
        stateList.Clear();
        requestedStates.Clear();
    }
    /// //////////////////////////////////////////
    //Use this when a non-state function wants to transition states
    public void RequestStateExternally(State requestedState)
    {
        requestedStates.Enqueue(requestedState);
    }
    public bool StateHasBeenRequested
    {
        get
        {
            if (requestedStates.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
    public State PeekRequested()
    {
        if (requestedStates.Count > 0)
        {
            return requestedStates.Peek();
        } else
        {
            return null;
        }
    }
    public void DequeueTopRequested()
    {
        requestedStates.Dequeue();
    }
    public void PushRequestToActive()
    {
        stateList.Push(requestedStates.Peek());
        requestedStates.Dequeue();
    }
    public void PushAndExecuteRequest()
    {
        stateList.Push(requestedStates.Peek());
        requestedStates.Dequeue();
        stateList.Peek()();
    }
    public void PrintQueue()
    {
        Debug.Log("State queue length: " + stateList.Count);
        foreach(State state in stateList)
        {
            Debug.Log(state.Method.Name);
        }
    }
}