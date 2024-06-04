using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// Keeps track of the current State being processed by the Battle Manager.
    /// </summary>
    private int currentStateIndex;
    private int currentEntityIndex;
    private StateOld currentState;
    private BattleGrid battleGrid;
    public PlayerInput playerInput;
    [SerializeField] private List<BattleEntityStats> turnOrder;

    [SerializeField] private string diceString;

    //Singleton pattern- useful when there is only one instance of an item
    /// <summary>
    /// Instance reference to BattleManager.
    /// </summary>
    public static BattleManager Instance { get; private set; }

    public BattleGrid BattleGridProperty { get { return battleGrid; } }


    private void Awake()
    {
        Debug.Log("BattleManager Awake Method Ran!");
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of BattleManager!", transform.gameObject);
        }

        Instance = this;
        battleGrid = new BattleGrid(new Vector2(-192, -60f), 12, 8, 32, 16);
        currentEntityIndex = 0;
        currentStateIndex = 0;

        if (turnOrder.Count == 0)
        {
            Debug.LogWarning("Battle Manager does not have any Battle Entities assigned to it!", transform.gameObject);
            turnOrder = new List<BattleEntityStats>();
        }

        diceString = "2d4+3";

    }

    private void OnEnable()
    {
        Debug.Log("BattleManager's OnEnable Ran!");
        Dice dice = new Dice(diceString);
        Debug.Log(dice.RollDice());
    }

    // Start is called before the first frame update
    private void Start()
    {
        BattleEntityStats testing = turnOrder[0];
        /*foreach (var stat in testing.CurrentStats)
        {
            Debug.Log(stat.Key+ " = "+ testing.GetModifiedStat(stat.Key));
        }*/
        /*testing.AddNewAddModifier(Constants.Keys_Stats.KEY_MAX_HEALTH, -10);
        Debug.Log(Constants.Keys_Stats.KEY_MAX_HEALTH + " = " + testing.GetModifiedStat(Constants.Keys_Stats.KEY_MAX_HEALTH));
        testing.AddNewMultiplyModifier(Constants.Keys_Stats.KEY_MAX_HEALTH, 0.5);
        testing.AddNewMultiplyModifier(Constants.Keys_Stats.KEY_MAX_HEALTH, -1);
        Debug.Log(Constants.Keys_Stats.KEY_MAX_HEALTH + " = " + testing.GetModifiedStat(Constants.Keys_Stats.KEY_MAX_HEALTH));*/
        /*AddState(turnOrder[0].DefaultState);
        currentState = transform.GetChild(0).GetComponent<State>();
        currentState.gameObject.SetActive(true);
        currentState.StartState();*/
    }

    public void NextBattleEntity()
    {
        //First get proper index in turn order
        currentEntityIndex = currentEntityIndex == turnOrder.Count - 1 ? 0 : currentEntityIndex + 1;

        //Then add its default behavior
        //AddState(turnOrder[currentEntityIndex].DefaultState);
        //As GameObjects are delayed when destroyed, technically the previous BattleEntity's states will
        //still exist in Battle Manager's list of states. The child count will include these states as well
        //To account for this, use childCount -1 as the next Battle Entity's states will be at the end of the
        //list
        //transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
        currentState = transform.GetChild(transform.childCount - 1).gameObject.GetComponent<StateOld>();
        currentState.gameObject.SetActive(true);
        currentState.StartState();
    }

    /// <summary>
    /// Add individual state to end of Battle Manager's state list.
    /// </summary>
    /// <param name="state">State that will be added.</param>
    public void AddState(StateOld state)
    {
        Transform stateClone = Instantiate(state.transform);
        stateClone.SetParent(transform, false);
    }

    /// <summary>
    /// Moves to the previous state in the Battle Manager's list of states.
    /// </summary>
    public void PreviousState()
    {
        if (transform.childCount > 1)
        {
            //Clean up current state, and then transition to previous state
            currentState.EndState();

            //Destroys current state (so it clears logic flow for any new states the previous state
            //might introduce)
            Destroy(currentState.gameObject);

            currentStateIndex--;
            currentState = transform.GetChild(currentStateIndex).gameObject.GetComponent<StateOld>();
            currentState.StartState();
        }
    }

    /// <summary>
    /// Moves to the next state in the Battle Manager's list of states (if anymore are found).
    /// </summary>
    public void NextState()
    {
        if (currentStateIndex < transform.childCount - 1)
        {
            //Clean up current state, and then transition to next state
            currentState.EndState();
            currentStateIndex++;
            currentState = transform.GetChild(currentStateIndex).gameObject.GetComponent<StateOld>();
            currentState.gameObject.SetActive(true);
            currentState.StartState();
        }
        else
        {
            //Clean up last state, remove all current states and transition to next
            //BattleEntity in turn order
            currentState.EndState();
            foreach (Transform child in transform.GetComponentInChildren<Transform>())
            {
                Destroy(child.gameObject);
            }
            currentStateIndex = 0;
            NextBattleEntity();
        }
    }

}
