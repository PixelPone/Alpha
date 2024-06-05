using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool IsComplete { get; protected set; }

    protected float startTime;

    public float CurrentDuration => Time.time - startTime;

    protected Animator MachineCoreAnimator => machineCore.animator;
    protected StateMachineCore machineCore;

    //Remember that States can have their own State Machines as well
    //StateMachineCore = the State Machine that is at the very top of the State Machine hierarchy
    public StateMachine stateMachine;
    public State CurrentState => stateMachine.currentState;


    public void SetMachineCore(StateMachineCore newCore)
    {
        machineCore = newCore;
    }

    protected void SetState(State newState, bool forceReset = false)
    {
        stateMachine.SetState(newState, forceReset);
    }

    public void InitializeState()
    {
        IsComplete = false;
        startTime = Time.time;
    }

    public void UpdateBranch()
    {
        StateUpdate();
        if(CurrentState != null)
        {
            CurrentState.UpdateBranch();
        }
    }

    public abstract void Enter();

    public abstract void StateUpdate();

    public abstract void StateFixedUpdate();

    public abstract void Exit();

}
