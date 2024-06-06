using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState;

    public void SetState(State newState, bool forcedRestart = false)
    {
        if (currentState != newState || forcedRestart)
        {
            if (currentState != null)
            {
                currentState.StateExit();
            }
            currentState = newState;
            currentState.InitializeState(this);
            currentState.StateEnter();
        }
    }
}
