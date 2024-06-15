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
public abstract class CombatEvent : MonoBehaviour
{
    /// <summary>
    /// Represents how soon the CombatEvent is due to execute.
    /// </summary>
    /// <remarks>
    /// <para>
    ///  The CombatEvent with the lowest CountDown value is the one that is set to execute next- it is removed from 
    ///  the CombatQueue and all the other CountDown values are decreased by one.
    /// </para>
    ///  <para>
    ///  Note!- CountDown is only used to determine the order of a CombatEvent, not how much time it will take for 
    ///  a CombatEvent to run.
    /// </para>
    /// </remarks>
    public int CountDown { get; set; }

    /// <summary>
    /// Indicates “who” owns the CombatEvent. Useful when removing all CombatEvents associated with a certain owner from
    /// CombatQueue.
    /// </summary>
    public GameObject Owner { get; private set; }

    /// <summary>
    /// Runs when the CombatEvent is first run (can be used setup values need for CombatEvent)
    /// </summary>
    /// <remarks>
    /// This is equivalent to the Start method for Monobehaviour but is being explicitly called 
    /// instead of being run on its own. It also can be run multiple times
    /// </remarks>
    public abstract void StartEvent();

    /// <summary>
    /// Updates components associated with the CombatEvent every frame.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the Update method for Monobehaviour, but is being explicitly called 
    /// instead of being run on its own. 
    /// </remarks>
    public abstract void UpdateEvent();

    /// <summary>
    /// Cleans up the logic for the CombatEvent.
    /// </summary>
    public abstract void EndEvent();

    /// <summary>
    /// Indicates if the CombatEvent is finished.
    /// </summary>
    /// <returns>
    /// True- if the CombatEvent is finished.
    /// False- if the CombatEvent is not finished.
    /// </returns>
    public abstract bool IsFinished();

}
