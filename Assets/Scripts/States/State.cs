using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool IsComplete { get; protected set; }

    protected float startTime;

    public float CurrentDuration => Time.time - startTime;

    public abstract void StateEnter();

    public abstract void StateUpdate();

    public abstract void StateFixedUpdate();

    public abstract void StateExit();
}
