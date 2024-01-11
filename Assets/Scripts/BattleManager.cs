using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] private List<GameObject> turnOrder;
    //private Queue<GameObject> actionQueue;

    //Singleton pattern- useful when there is only one instance of an item
    public static BattleManager Instance { get; private set; }


    private void Awake()
    {
        Debug.Log(gameObject.name + "'s BattleManager Awake Method Ran!");
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of BattleManager!", transform.gameObject);
        }
        Instance = this;

        if (turnOrder.Count == 0)
        {
            Debug.LogWarning("Battle Manager does not have any Battle Entities assigned to it!", transform.gameObject);
            turnOrder = new List<GameObject>();
        }
    }

    private void OnEnable()
    {
        Debug.Log(gameObject.name + "'s BattleManager's OnEnable Ran!");
    }

    // Start is called before the first frame update
    private void Start()
    {
        turnOrder[0].SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

}
