using Assets.Scripts.Actors;
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
    /// Maintains the CombatStates that are waiting to be executed in battle.
    /// </summary>
    /// <remarks>
    /// CombatStates at the front of the queue are executed first (have lower CountDown values)
    /// CombatStates are inserted and ordered by their CountDown value.
    /// </remarks>
    private List<CombatState> combatStateQueue;

    /// <summary>
    /// Any CombatState substates that are associated with the current CombatState being run
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
        combatStateQueue = new List<CombatState>();
        substateQueue = new List<CombatState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get combat order, and add initial CombatStates to CombatStateQueue
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
        //Remove latest CombatEvent from CombatStateQueue and add it to Substate Queue
        CombatState front = combatStateQueue[0];
        combatStateQueue.RemoveAt(0);
        AddSubstate(front.Owner, front);

        //Start this new Substate
        currentSubstate = substateQueue[subStateIndex];
        currentSubstate.StartState(this);

        //Update all CountDowns for CombatStates in CombatStateQueue
        foreach (CombatState combatEvent in combatStateQueue)
        {
            combatEvent.CountDown = Mathf.Max(0, combatEvent.CountDown - 1);
        }
    }

    /// <summary>
    /// Adds a CombatState to its proper place in the CombatStateQueue.
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
    /// <param name="actorOwner">The owner that is associated with the new CombatState being added.</param>
    /// <param name="newMainState">The new CombatState that is being added to the CombatStateQueue.</param>
    /// <param name="eventCountDown">The CountDown that is associated with the CombatState.</param>
    public void AddCombatEvent(ActorStats actorOwner, CombatState newMainState, int eventCountDown)
    {
        if(IsEmpty())
        {
            newMainState.CountDown = eventCountDown;
            newMainState.Owner = actorOwner;
            combatStateQueue.Add(newMainState);
        }
        else
        {
            int properIndex = 0;
            for (int i = 0; i < combatStateQueue.Count; i++)
            {
                newMainState.CountDown = eventCountDown;
                newMainState.Owner = actorOwner;

                CombatState current = combatStateQueue[i];
                if (current.CountDown < eventCountDown)
                {
                    properIndex++;
                }
            }

            combatStateQueue.Insert(properIndex, newMainState);
        }
    }

    /// <summary>
    /// Checks if an Actor has at least one CombatState associated with it currently in the CombatStateQueue.
    /// </summary>
    /// <remarks>
    /// Note!- This does not include any substates that are currently being processed in the Substate Queue.
    /// Might change this for later.
    /// </remarks>
    /// <param name="actor">The Actor that is being looked for.</param>
    /// <returns>
    /// True- if at least one CombatState is found in the current CombatStateQueue.
    /// False- if no CombatStates are found in the current CombatStateQueue.
    /// </returns>
    public bool DoesActorHaveCombatEvent(GameObject actor)
    {

        foreach (CombatState queueEvent in combatStateQueue)
        {
            if (queueEvent.Owner == actor)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Removes all CombatStates associated with an Actor in the CombatStateQueue.
    /// </summary>
    /// <param name="actor">The Actor whose events are getting removed.</param>
    public void RemoveEventsOwnedBy(GameObject actor)
    {

        for (int i = 0; i < combatStateQueue.Count; i++)
        {
            if (combatStateQueue[i].Owner == actor)
            {
                combatStateQueue.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Clears the entirety of the CombatStateQueue.
    /// </summary>
    public void ClearCombatEventEverything()
    {
        combatStateQueue.Clear();
    }

    /// <summary>
    /// Returns if the CombatStateQueue is currently empty.
    /// </summary>
    /// <remarks>
    /// This does not check for any substates in the 
    /// even if the current CombatStateQueue is empty.
    /// </remarks>
    /// <returns>
    /// True- if no CombatStates are found in the current CombatStateQueue.
    /// False- if at least one CombatState is found in the current CombatStateQueue.
    /// </returns>
    public bool IsEmpty()
    {
        return combatStateQueue.Count == 0;
    }

    /// <summary>
    /// Prints the current CombatEvent as well as all the CombatStates that are currently in the CombatStateQueue.
    /// </summary>
    public void PrintQueue()
    {
        if (IsEmpty())
        {
            Debug.Log("The CombatStateQueue is empty!");
        }
        else
        {
            Debug.Log("CombatEvent Queue:");
            for (int i = 0; i < combatStateQueue.Count; i++)
            {
                CombatState current = combatStateQueue[i];
                Debug.Log("[" + i + "] CombatStateQueue: [" + current.CountDown + "][" + current + "]");
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
            //Clean up current Substate, and then transition to previous Substate
            currentSubstate.EndState();

            //Remove current state (so it clears logic flow for any new states the previous state
            //might introduce)
            substateQueue.RemoveAt(subStateIndex);

            //Then get previous Substate and start its logic again
            subStateIndex--;
            currentSubstate = substateQueue[subStateIndex];
            currentSubstate.StartState(this);
        }
    }

    public void NextSubstate()
    {
        if(subStateIndex < substateQueue.Count - 1)
        {
            //Clean up current Substate, and then transition to next Substate
            currentSubstate.EndState();
            subStateIndex++;
            currentSubstate = substateQueue[subStateIndex];
            currentSubstate.StartState(this);
        }
        else
        {
            //Clean up last Substate, remove all current substates and transition to next CombatState
            currentSubstate.EndState();
            subStateIndex = 0;
            currentSubstate = null;
            substateQueue.Clear();
            //Transition to next CombatState is currently being handled by Update- might change later to be here
        }
    }

}
