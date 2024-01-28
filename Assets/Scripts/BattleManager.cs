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
    private BattleGrid battleGrid;
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] private List<BattleEntity> turnOrder;

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
            turnOrder = new List<BattleEntity>();
        }

    }

    private void OnEnable()
    {
        Debug.Log("BattleManager's OnEnable Ran!");
    }

    // Start is called before the first frame update
    private void Start()
    {
        AddBehavior(turnOrder[0].DefaultBehavior);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void NextBattleEntity()
    {
        //First get proper index in turn order
        currentEntityIndex = currentEntityIndex == turnOrder.Count - 1 ? 0 : currentEntityIndex + 1;

        //Then add its default behavior
        AddBehavior(turnOrder[currentEntityIndex].DefaultBehavior);
        //As GameObjects are delayed when destroyed, technically the previous BattleEntity's states will
        //still exist in Battle Manager's list of states. The child count will include these states as well
        //To account for this, use childCount -1 as the next Battle Entity's states will be at the end of the
        //list
        transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
    }

    /// <summary>
    /// Add behavior (list of associated state GameObjects) to end of Battle Manager's state list.
    /// </summary>
    /// <param name="behaviorObject">The parent GameObject whose children are the list of state GameObjects to be added.</param>
    public void AddBehavior(GameObject behaviorObject)
    {
        //Get list of state GameObjects associated with behavior except for behavior object itself
        IEnumerable<Transform> behaviorStates = behaviorObject.GetComponentsInChildren<Transform>(true).Where(t =>
        {
            return t != behaviorObject.transform;
        });

        foreach (Transform t in behaviorStates)
        {
            Transform stateClone = Instantiate(t);
            stateClone.SetParent(transform, false);
        }
    }

    /// <summary>
    /// Add individual state to end of Battle Manager's state list.
    /// </summary>
    /// <param name="state">State GameObject that will be added.</param>
    public void AddState(GameObject state)
    {
        Transform stateClone = Instantiate(state.transform);
        stateClone.SetParent(transform, false);
    }

    /// <summary>
    /// Moves to the previous state in the Battle Manager's list of states.
    /// </summary>
    public void PreviousState()
    {
        //Clean up current state, and then transition to previous state
        GameObject currentGameObject = transform.GetChild(currentStateIndex).gameObject;
        currentGameObject.SetActive(false);
        //Destroys current state (so it clears logic flow for any new states the previous state
        //might introduce)
        Destroy(currentGameObject);
        currentStateIndex--;
        transform.GetChild(currentStateIndex).gameObject.SetActive(true);
    }

    /// <summary>
    /// Moves to the next state in the Battle Manager's list of states (if anymore are found).
    /// </summary>
    public void NextState()
    {
        if (currentStateIndex < transform.childCount - 1)
        {
            //Clean up current state, and then transition to next state
            GameObject currentGameObject = transform.GetChild(currentStateIndex).gameObject;
            currentGameObject.SetActive(false);
            currentStateIndex++;
            transform.GetChild(currentStateIndex).gameObject.SetActive(true);
        }
        else
        {
            //Clean up last state, remove all current states and transition to next
            //BattleEntity in turn order
            transform.GetChild(currentStateIndex).gameObject.SetActive(false);
            foreach(Transform child in transform.GetComponentInChildren<Transform>())
            {
                Destroy(child.gameObject);
            }
            currentStateIndex = 0;
            NextBattleEntity();
        }
    }

}
