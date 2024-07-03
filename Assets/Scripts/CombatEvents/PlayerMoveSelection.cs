using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSelection : CombatState
{
    private MoveStateHelper moveStateHelper;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void PlayerInput_OnMoveAction(object sender, PlayerInput.InputActionArgs args)
    {
        Vector2 playerInput = args.callbackContext.ReadValue<Vector2>();
        if (playerInput == Vector2Int.left || playerInput == Vector2Int.right
            || playerInput == Vector2Int.up || playerInput == Vector2Int.down)
        {
            moveStateHelper.UpdateHoverPosition(playerInput);
        }
    }

    private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        moveStateHelper.AddMovementToStack();
    }

    public override void StartState(BattleManager battleManager)
    {
        base.StartState(battleManager);
        moveStateHelper = new MoveStateHelper(this.Owner, battleManager.BattleGridProperty);
        Debug.Log("MoveSelection's StartState method Ran!");
        PlayerInput.Instance.OnMoveAction += PlayerInput_OnMoveAction;
        PlayerInput.Instance.OnSelectAction += PlayerInput_OnSelectAction;
    }

    public override void UpdateState()
    {
        //throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        Debug.Log("MoveSelection's EndState Ran!");
        PlayerInput.Instance.OnMoveAction -= PlayerInput_OnMoveAction;
        PlayerInput.Instance.OnSelectAction -= PlayerInput_OnSelectAction;
    }

    public override bool IsFinished()
    {
        throw new System.NotImplementedException();
    }
}
