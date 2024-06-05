using UnityEngine;

public abstract class StateMachineCore : MonoBehaviour
{
    public Animator animator;
    public StateMachine stateMachine;

    public void SetUpInstances()
    {
        stateMachine = new StateMachine();
        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State childState in allChildStates)
        {
            childState.SetMachineCore(this);
        }
    }

    protected void SetState(State newState, bool forcedRestart = false)
    {
        stateMachine.SetState(newState, forcedRestart);
    }
}
