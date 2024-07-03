using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeTurn : CombatState
{

    private int menuIndex;

    [SerializeField]
    private CombatState moveSelection;

    private void Awake()
    {
        menuIndex = 0;
    }

    private void PlayerInput_OnMoveAction(object sender, PlayerInput.InputActionArgs args)
    {
        Vector2 currentInput = args.callbackContext.ReadValue<Vector2>();
        if (currentInput == Vector2.left)
        {
            if (menuIndex == 0)
            {
                menuIndex = 1;
            }
            else if (menuIndex == 2)
            {
                menuIndex = 3;
            }
            else
            {
                menuIndex--;
            }
        }
        if (currentInput == Vector2.right)
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
                menuIndex -= 2;
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
        string test = string.Empty;
        switch (menuIndex)
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
        if (test == "Move")
        {
            this.battleManager.AddSubstate(this.Owner, moveSelection);
            this.battleManager.NextSubstate();
        }
    }

    public override void StartState(BattleManager battleManager)
    {
        base.StartState(battleManager);
        Debug.Log("TakeTurn's StartState method Ran!");
        PlayerInput.Instance.OnMoveAction += PlayerInput_OnMoveAction;
        PlayerInput.Instance.OnSelectAction += PlayerInput_OnSelectAction;
    }

    public override void UpdateState()
    {
        //throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        Debug.Log("MainPlayerState's EndState Ran!");
        menuIndex = 0;
        PlayerInput.Instance.OnMoveAction -= PlayerInput_OnMoveAction;
        PlayerInput.Instance.OnSelectAction -= PlayerInput_OnSelectAction;
    }

    public override bool IsFinished()
    {
        throw new System.NotImplementedException();
    }
}
