using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelection : State
{

    private PlayerInput playerInput;

    public override void StateEnter()
    {
        playerInput = PlayerInput.Instance;
        playerInput.OnMoveAction += PlayerInput_OnMoveAction;
    }

    public override void StateExit()
    {
        playerInput.OnMoveAction -= PlayerInput_OnMoveAction;
    }

    public override void StateFixedUpdate()
    {
        throw new NotImplementedException();
    }

    public override void StateUpdate()
    {
        throw new NotImplementedException();
    }

    private void PlayerInput_OnMoveAction(object sender, PlayerInput.InputActionArgs args)
    {
        Vector2 playerInput = args.callbackContext.ReadValue<Vector2>();
        Debug.Log(playerInput.normalized);
    }
}
