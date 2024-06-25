using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// The list of Actors that are currently in battle.
    /// </summary>
    [SerializeField] private List<ActorStats> entityList;

    /// <summary>
    /// Current CombatState that is running from the queue-
    /// this CombatState was the one previously at the front of the queue.
    /// </summary>
    private CombatState currentEvent;

    /// <summary>
    /// Maintains the CombatStates that currently running in battle.
    /// </summary>
    /// <remarks>
    /// CombatStates at the front of the queue are executed first (have lower CountDown values)
    /// CombatStates are inserted and ordered by their CountDown value.
    /// </remarks>
    private List<CombatState> combatEventQueue;


    private List<CombatState> substateQueue;

    private BattleGrid battleGrid;

    private void Awake()
    {
        battleGrid = new BattleGrid(new Vector2(-192, -60f), 12, 8, 32, 16);
        if (entityList.Count == 0)
        {
            Debug.LogWarning("Battle Manager does not have any Actors assigned to it!", transform.gameObject);
            entityList = new List<ActorStats>();
        }
        combatEventQueue = new List<CombatState>();
        substateQueue = new List<CombatState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get combat order, and add initial CombatStates to CombatEventQueue
        GetInitiative();
        for (int i = 0; i < entityList.Count; i++)
        {
            CombatState entityState = entityList[i].InitialCombatEvent;
            AddCombatEvent(entityState, i);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Sets up the initial turn order for a battle.
    /// </summary>
    private void GetInitiative()
    {
        Dice initiativeModifier = new Dice("1d10");
        Dice coin = new Dice("1d2");

        entityList.Sort(delegate (ActorStats one, ActorStats two)
        {
            //Negative value of CompareTo is returned in order to make Actors with higher stats toward front of order
            //Have to use BASE Perception when calculating Initiative rolls
            int initiativeTotalOne = one.BaseActorSpecial.Perception + initiativeModifier.RollDice();
            int initiativeTotalTwo = two.BaseActorSpecial.Perception + initiativeModifier.RollDice();

            int compare = initiativeTotalOne.CompareTo(initiativeTotalTwo);
            if (compare != 0)
            {
                return -compare;
            }

            //If Initiative rolls are both equal, next compare Agility
            compare = one.Agility.CompareTo(two.Agility);
            if (compare != 0)
            {
                return -compare;
            }

            //If Agilities are both equal, then next use Luck
            compare = one.Luck.CompareTo(two.Luck);
            if (compare != 0)
            {
                return -compare;
            }

            //If all fails, then just flip a coin for position
            int coinResult = coin.RollDice();
            if (coinResult == 1)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        });
    }

    /// <summary>
    /// Adds a CombatState to its proper place in the CombatEventQueue.
    /// </summary>
    /// <remarks>
    /// <para>
    /// CombatStates are ordered based on their CountDown value (how soon they are expected to execute). 
    /// Adding goes like this:
    /// <br></br>
	/// If CountDown == -1, add it to the front of the queue(after all events that have -1),<br></br>
	/// If CountDown != -1, iterate through the queue until a CombatState with a higherCountDown has been found, 
    /// and then insert CombatState right before it.
    /// </para>
    /// </remarks>
    /// <param name="newEvent">The new CombatState that is being added to the CombatEventQueue.</param>
    /// <param name="eventCountDown">The CountDown that is associated with the CombatState.</param>
    public void AddCombatEvent(CombatState newEvent, int eventCountDown)
    {
        for (int i = 0; i < combatEventQueue.Count; i++)
        {
            CombatState current = combatEventQueue[i];
            if (current.CountDown > eventCountDown)
            {
                combatEventQueue.Insert(i, newEvent);
            }
        }
    }

    /// <summary>
    /// Checks if an Actor has at least one CombatState associated with it currently in the CombatEventQueue.
    /// </summary>
    /// <remarks>
    /// Note!- When I refer to CombatEventQueue here, I am referring to both to currentEvent and the CombatEventQueue itself
    /// as currentEvent is just the previous head of the CombatEventQueue.
    /// </remarks>
    /// <param name="actor">The Actor that is being looked for.</param>
    /// <returns>
    /// True- if at least one CombatState is found in the current CombatEventQueue.
    /// False- if no CombatStates are found in the current CombatEventQueue.
    /// </returns>
    public bool DoesActorHaveCombatEvent(GameObject actor)
    {

        if (currentEvent.Owner == actor)
        {
            return true;
        }

        foreach (CombatState queueEvent in combatEventQueue)
        {
            if (queueEvent.Owner == actor)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Removes all CombatStates associated with an Actor in the CombatEventQueue.
    /// </summary>
    /// <param name="actor">The Actor whose events are getting removed.</param>
    public void RemoveEventsOwnedBy(GameObject actor)
    {

        for (int i = 0; i < combatEventQueue.Count; i++)
        {
            if (combatEventQueue[i].Owner == actor)
            {
                combatEventQueue.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Clears the entirety of the CombatEventQueue, including currentEvent.
    /// </summary>
    public void ClearEverything()
    {
        combatEventQueue.Clear();
        currentEvent = null;
    }

    /// <summary>
    /// Returns if the CombatEventQueue is currently empty
    /// </summary>
    /// <remarks>
    /// This does not check for currentEvent- meaning that technically there can be a single CombatState
    /// even if the current CombatEventQueue is empty.
    /// </remarks>
    /// <returns>
    /// True- if no CombatStates are found in the current CombatEventQueue.
    /// False- if at least one CombatState is found in the current CombatEventQueue.
    /// </returns>
    public bool IsEmpty()
    {
        return combatEventQueue.Count == 0;
    }

    /// <summary>
    /// Prints the current CombatState as well as all the CombatStates that are currently in the CombatEventQueue.
    /// </summary>
    public void PrintQueue()
    {
        if (IsEmpty())
        {
            Debug.Log("The CombatEventQueue is empty!");
        }
        else
        {
            Debug.Log("Current CombatEvent: " + currentEvent);
            Debug.Log("CombatEvent Queue:");
            for (int i = 0; i < combatEventQueue.Count; i++)
            {
                CombatState current = combatEventQueue[i];
                Debug.Log("[" + i + "] CombatEventQueue: [" + current.CountDown + "][" + current + "]");
            }
        }
    }

}
