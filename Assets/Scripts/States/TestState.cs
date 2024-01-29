using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : State
{

    private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        Debug.Log("This is from Test State!");
    }

    public override void StartState()
    {
        BattleManager.Instance.playerInput.OnSelectAction += PlayerInput_OnSelectAction;
    }

    public override void UpdateState(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        BattleManager.Instance.playerInput.OnSelectAction -= PlayerInput_OnSelectAction;
    }
}
