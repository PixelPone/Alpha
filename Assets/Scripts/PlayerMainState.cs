using UnityEngine;

/// <summary>
/// Default main state of a player in battle.
/// </summary>
public class PlayerMainState : MonoBehaviour
{

    private int menuIndex;

    [SerializeField] private GameObject moveSelectionState;


    private void Awake()
    {
        menuIndex = 0;
    }
    private void OnEnable()
    {
        Debug.Log("MainPlayerState's OnEnable Ran!");

        BattleManager.Instance.playerInput.OnMoveAction += PlayerInput_OnMoveAction;
        BattleManager.Instance.playerInput.OnSelectAction += PlayerInput_OnSelectAction;
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    private void PlayerInput_OnMoveAction(object sender, PlayerInput.InputActionArgs args)
    {
        Debug.Log("OnMoveAction Ran in PlayerMainState!");
        Vector2 currentInput = args.callbackContext.ReadValue<Vector2>();
        if(currentInput == Vector2.left)
        {
            if (menuIndex == 0)
            {
                menuIndex = 1;
            }
            else if(menuIndex == 2)
            {
                menuIndex = 3;
            }
            else
            {
                menuIndex--;
            }
        }
        if(currentInput == Vector2.right)
        {
            if (menuIndex == 1)
            {
                menuIndex = 0;
            }
            else if (menuIndex == 3)
            {
                menuIndex = 2;
            }
            else
            {
                menuIndex++;
            }
        }
        if (currentInput == Vector2.up)
        {
            if (menuIndex == 0)
            {
                menuIndex = 2;
            }
            else if (menuIndex == 1)
            {
                menuIndex = 3;
            }
            else
            {
                menuIndex-=2;
            }
        }
        if (currentInput == Vector2.down)
        {
            if (menuIndex == 2)
            {
                menuIndex = 0;
            }
            else if (menuIndex == 3)
            {
                menuIndex = 1;
            }
            else
            {
                menuIndex += 2;
            }
        }
    }

    private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        Debug.Log("SelectionAction Ran in PlayerMainState!");
        string test = string.Empty;
        switch(menuIndex)
        {
            case 0:
                test = "Move";
                break;
            case 1:
                test = "Attack";
                break;
            case 2:
                test = "Item";
                break;
            case 3:
                test = "Flee";
                break;
        }

        Debug.Log(test);
        if(menuIndex == 0)
        {
            BattleManager.Instance.AddState(moveSelectionState);
            BattleManager.Instance.NextState();
        }
    }

    private void OnDisable()
    {
        Debug.Log("MainPlayerState's OnDisable Ran!");
        BattleManager.Instance.playerInput.OnMoveAction -= PlayerInput_OnMoveAction;
        BattleManager.Instance.playerInput.OnSelectAction -= PlayerInput_OnSelectAction;
    }

}
