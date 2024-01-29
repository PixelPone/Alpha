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
/// <seealso cref="TurnMachine"/>
public abstract class State : MonoBehaviour
{

    /// <summary>
    /// Sets up the logic for the state.
    /// </summary>
    public abstract void StartState();

    /// <summary>
    /// Updates the logic for the state every frame.
    /// </summary>
    public abstract void UpdateState(float deltaTime);

    /// <summary>
    /// Cleans up the logic for the state.
    /// </summary>
    public abstract void EndState();
}