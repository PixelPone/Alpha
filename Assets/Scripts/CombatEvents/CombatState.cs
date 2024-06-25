using UnityEngine;

/// <summary>
/// Represents a phase of battle.
/// </summary>
/// <remarks>
/// <para>
///  In Unity, there is no set order for which objects call their Awake, Start, and Update methods
///  (even for a GameObject with child GameObjects). This means that a child's Awake method can
///  run before its parent's, leading to potential null reference errors when initializing.
/// </para>
///  <para>
///  In the context of this state machine system, it means that is possible for a state's Start method to run 
///  before the Start method of its associated state machine. To control the order of initilization and updating so 
///  that state machines' methods run before their states, these methods will be explicitly called using this class.
/// </para>
/// </remarks>
public abstract class CombatState : MonoBehaviour
{
    /// <summary>
    /// Represents how soon the CombatState is due to execute.
    /// </summary>
    /// <remarks>
    /// <para>
    ///  The CombatState with the lowest CountDown value is the one that is set to execute next- it is removed from 
    ///  the CombatEventQueue and all the other CountDown values are decreased by one.
    /// </para>
    ///  <para>
    ///  Note!- CountDown is only used to determine the order of a CombatState, not how much time it will take for 
    ///  a CombatState to run.
    /// </para>
    /// </remarks>
    public int CountDown { get; set; }

    /// <summary>
    /// The time in which this CombatState started.
    /// </summary>
    protected float startTime;

    /// <summary>
    /// The duration of how long this CombatState has been running for.
    /// </summary>
    public float CurrentDuration => Time.time - startTime;

    /// <summary>
    /// Indicates “who” owns the CombatState. Useful when removing all CombatStates associated with a certain owner from
    /// CombatEventQueue.
    /// </summary>
    public GameObject Owner { get; private set; }

    /// <summary>
    /// Runs when the CombatState is first run (can be used setup values need for CombatState)
    /// </summary>
    /// <remarks>
    /// This is equivalent to the Start method for Monobehaviour but is being explicitly called 
    /// instead of being run on its own. It also can be run multiple times.
    /// </remarks>
    /// <param name="battleManager">The BattleManager that is associated with this CombatState.</param>
    public virtual void StartState(BattleManager battleManager)
    {
        startTime = Time.time;
    }

    /// <summary>
    /// Updates components associated with the CombatState every frame.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the Update method for Monobehaviour, but is being explicitly called 
    /// instead of being run on its own. 
    /// </remarks>
    public abstract void UpdateState();

    /// <summary>
    /// Cleans up the logic for the CombatState.
    /// </summary>
    public abstract void EndState();

    /// <summary>
    /// Indicates if the CombatState is finished.
    /// </summary>
    /// <returns>
    /// True- if the CombatState is finished.
    /// False- if the CombatState is not finished.
    /// </returns>
    public abstract bool IsFinished();

}
