using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    [SerializeField] private List<ActorStats> turnOrder;

    /// <summary>
    /// Maintains the CombatEvents that currently running in battle.
    /// </summary>
    /// <remarks>
    /// CombatEvents at the front of the queue are executed first (have lower CountDown values)
    /// CombatEvents are inserted and ordered by their CountDown value.
    /// </remarks>
    private List<CombatEvent> combatQueue;
    /// <summary>
    /// Current CombatEvent that is running from the queue- 
    /// this CombatEvent was the one previously at the front of the queue.
    /// </summary>
    private CombatEvent currentEvent;

    private BattleGrid battleGrid;

    private void Awake()
    {
        battleGrid = new BattleGrid(new Vector2(-192, -60f), 12, 8, 32, 16);
        if (turnOrder.Count == 0)
        {
            Debug.LogWarning("Battle Manager does not have any Actors assigned to it!", transform.gameObject);
            turnOrder = new List<ActorStats>();
        }
        combatQueue = new List<CombatEvent>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Adds a CombatEvent to its proper place in the CombatQueue.
    /// </summary>
    /// <remarks>
    /// <para>
    /// CombatEvents are ordered based on their CountDown value (how soon they are expected to execute). 
    /// Adding goes like this:
    /// <br></br>
	/// If CountDown == -1, add it to the front of the queue(after all events that have -1),<br></br>
	/// If CountDown != -1, iterate through the queue until a CombatEvent with a higherCountDown has been found, 
    /// and then insert CombatEvent right before it.
    /// </para>
    /// </remarks>
    /// <param name="newEvent">The new CombatEvent that is being added to the CombatQueue.</param>
    /// <param name="eventCountDown">The CountDown that is associated with the CombatEvent.</param>
    public void AddCombatEvent(CombatEvent newEvent, int eventCountDown)
    {
        for (int i = 0; i < combatQueue.Count; i++)
        {
            CombatEvent current = combatQueue[i];
            if (current.CountDown > eventCountDown)
            {
                combatQueue.Insert(i, newEvent);
            }
        }
    }

    /// <summary>
    /// Checks if an Actor has at least one CombatEvent associated with it currently in the CombatQueue.
    /// </summary>
    /// <remarks>
    /// Note!- When I refer to CombatQueue here, I am referring to both to currentEvent and the CombatQueue itself
    /// as currentEvent is just the previous head of the CombatQueue.
    /// </remarks>
    /// <param name="actor">The Actor that is being looked for.</param>
    /// <returns>
    /// True- if at least one CombatEvent is found in the current CombatQueue.
    /// False- if no CombatEvents are found in the current CombatQueue.
    /// </returns>
    public bool DoesActorHaveCombatEvent(GameObject actor)
    {

        if (currentEvent.Owner == actor)
        {
            return true;
        }

        foreach (CombatEvent queueEvent in combatQueue)
        {
            if (queueEvent.Owner == actor)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Removes all CombatEvents associated with an Actor in the CombatQueue.
    /// </summary>
    /// <param name="actor">The Actor whose events are getting removed.</param>
    public void RemoveEventsOwnedBy(GameObject actor)
    {

        for (int i = 0; i < combatQueue.Count; i++)
        {
            if (combatQueue[i].Owner == actor)
            {
                combatQueue.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Clears the entirety of the CombatQueue, including currentEvent.
    /// </summary>
    public void ClearEverything()
    {
        combatQueue.Clear();
        currentEvent = null;
    }

    /// <summary>
    /// Returns if the CombatQueue is currently empty
    /// </summary>
    /// <remarks>
    /// This does not check for currentEvent- meaning that technically there can be a single CombatEvent
    /// even if the current CombatQueue is empty.
    /// </remarks>
    /// <returns>
    /// True- if no CombatEvents are found in the current CombatQueue.
    /// False- if at least one CombatEvent is found in the current CombatQueue.
    /// </returns>
    public bool IsEmpty()
    {
        return combatQueue.Count == 0;
    }

    /// <summary>
    /// Prints the current CombatEvent as well as all the CombatEvents that are currently in the CombatQueue.
    /// </summary>
    public void PrintQueue()
    {
        if (IsEmpty())
        {
            Debug.Log("The CombatQueue is empty!");
        }
        else
        {
            Debug.Log("Current CombatEvent: " + currentEvent);
            Debug.Log("CombatEvent Queue:");
            for (int i = 0; i < combatQueue.Count; i++)
            {
                CombatEvent current = combatQueue[i];
                Debug.Log("[" + i + "] combatQueue: [" + current.CountDown + "][" + current + "]");
            }
        }
    }

}
