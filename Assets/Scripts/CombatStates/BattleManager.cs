using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    private int subStateIndex;

    /// <summary>
    /// The list of Actors that are currently in battle.
    /// </summary>
    [SerializeField] private List<ActorStats> entityList;

    /// <summary>
    /// Maintains the CombatEvents that currently running in battle.
    /// </summary>
    /// <remarks>
    /// CombatStates at the front of the queue are executed first (have lower CountDown values)
    /// CombatStates are inserted and ordered by their CountDown value.
    /// </remarks>
    private List<CombatState> combatEventQueue;

    /// <summary>
    /// Any substates (CombatStates) that are associated with the current CombatEvent being run
    /// </summary>
    private List<CombatState> substateQueue;
    private CombatState currentSubstate;

    public BattleGrid BattleGridProperty { get; private set; }

    private void Awake()
    {
        subStateIndex = 0;
        currentSubstate = null;
        BattleGridProperty = new BattleGrid(new Vector2(-192, -60f), 12, 8, 32, 16);
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
            ActorStats owner = entityState.Owner;

            AddCombatEvent(owner, entityState, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSubstate != null)
        {
            currentSubstate.UpdateState();
        }
        else if(IsEmpty())
        {
            return;
        }
        else
        {
            GetNextCombatEvent();
        }
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

    private void GetNextCombatEvent()
    {
        //Remove latest CombatEvent from Queue and add it to Substate Queue
        CombatState front = combatEventQueue[0];
        combatEventQueue.RemoveAt(0);
        AddSubstate(front.Owner, front);

        //Start this new Substate
        currentSubstate = substateQueue[subStateIndex];
        currentSubstate.StartState(this);

        //Update all CountDowns for CombatEvents in CombatEventQueue
        foreach (CombatState combatEvent in combatEventQueue)
        {
            combatEvent.CountDown = Mathf.Max(0, combatEvent.CountDown - 1);
        }
    }

    /// <summary>
    /// Adds a CombatEvent to its proper place in the CombatEventQueue.
    /// </summary>
    /// <remarks>
    /// <para>
    /// CombatEvent are ordered based on their CountDown value (how soon they are expected to execute). 
    /// Adding goes like this:
    /// <br></br>
	/// If CountDown == -1, add it to the front of the queue(after all events that have -1),<br></br>
	/// If CountDown != -1, iterate through the queue until a CombatState with a higherCountDown has been found, 
    /// and then insert CombatState right before it.
    /// </para>
    /// </remarks>
    /// <param name="actorOwner">The owner that is associated with the new CombatEvent being added.</param>
    /// <param name="newEvent">The new CombatEvent that is being added to the CombatEventQueue.</param>
    /// <param name="eventCountDown">The CountDown that is associated with the CombatEvent.</param>
    public void AddCombatEvent(ActorStats actorOwner, CombatState newEvent, int eventCountDown)
    {
        if(IsEmpty())
        {
            newEvent.CountDown = eventCountDown;
            newEvent.Owner = actorOwner;
            combatEventQueue.Add(newEvent);
        }
        else
        {
            for (int i = 0; i < combatEventQueue.Count; i++)
            {
                newEvent.CountDown = eventCountDown;
                newEvent.Owner = actorOwner;

                CombatState current = combatEventQueue[i];
                if (current.CountDown > eventCountDown)
                {
                    combatEventQueue.Insert(i, newEvent);
                }
            }
        }
    }

    /// <summary>
    /// Checks if an Actor has at least one CombatEvent associated with it currently in the CombatEventQueue.
    /// </summary>
    /// <remarks>
    /// Note!- This does not include any substates that are currently being processed in the subState queue.
    /// Might change this for later.
    /// </remarks>
    /// <param name="actor">The Actor that is being looked for.</param>
    /// <returns>
    /// True- if at least one CombatEvent is found in the current CombatEventQueue.
    /// False- if no CombatEvents are found in the current CombatEventQueue.
    /// </returns>
    public bool DoesActorHaveCombatEvent(GameObject actor)
    {

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
    /// Removes all CombatEvents associated with an Actor in the CombatEventQueue.
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
    public void ClearCombatEventEverything()
    {
        combatEventQueue.Clear();
    }

    /// <summary>
    /// Returns if the CombatEventQueue is currently empty
    /// </summary>
    /// <remarks>
    /// This does not check for currentEvent- meaning that technically there can be a single CombatState
    /// even if the current CombatEventQueue is empty.
    /// </remarks>
    /// <returns>
    /// True- if no CombatEvents are found in the current CombatEventQueue.
    /// False- if at least one CombatState is found in the current CombatEventQueue.
    /// </returns>
    public bool IsEmpty()
    {
        return combatEventQueue.Count == 0;
    }

    /// <summary>
    /// Prints the current CombatEvent as well as all the CombatEvents that are currently in the CombatEventQueue.
    /// </summary>
    public void PrintQueue()
    {
        if (IsEmpty())
        {
            Debug.Log("The CombatEventQueue is empty!");
        }
        else
        {
            Debug.Log("CombatEvent Queue:");
            for (int i = 0; i < combatEventQueue.Count; i++)
            {
                CombatState current = combatEventQueue[i];
                Debug.Log("[" + i + "] CombatEventQueue: [" + current.CountDown + "][" + current + "]");
            }
        }
    }

    public void AddSubstate(ActorStats actorOwner, CombatState newSubstate)
    {
        newSubstate.Owner = actorOwner;
        substateQueue.Add(newSubstate);
    }

    public void PreviouSubstate()
    {
        if(substateQueue.Count > 1)
        {
            //Clean up current substate, and then transition to previous substate
            currentSubstate.EndState();

            //Remove current state (so it clears logic flow for any new states the previous state
            //might introduce)
            substateQueue.RemoveAt(subStateIndex);

            //Then get previous substate and start its logic again
            subStateIndex--;
            currentSubstate = substateQueue[subStateIndex];
            currentSubstate.StartState(this);
        }
    }

    public void NextSubstate()
    {
        if(subStateIndex < substateQueue.Count - 1)
        {
            //Clean up current substate, and then transition to next substate
            currentSubstate.EndState();
            subStateIndex++;
            currentSubstate = substateQueue[subStateIndex];
            currentSubstate.StartState(this);
        }
        else
        {
            //Clean up last substate, remove all current substates and transition to next CombatEvent
            currentSubstate.EndState();
            subStateIndex = 0;
            currentSubstate = null;
            substateQueue.Clear();
            //Transition to next CombatEvent is currently being handled by Update- might change later to be here
        }
    }

}
