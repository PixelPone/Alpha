using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelection : CombatState
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void PlayerInput_OnMoveAction(object sender, PlayerInput.InputActionArgs args)
    {

    }

    private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs args)
    {

    }

    public override void StartState(BattleManager battleManager)
    {
        base.StartState(battleManager);
        Debug.Log("MoveSelection's StartState method Ran!");
        PlayerInput.Instance.OnMoveAction += PlayerInput_OnMoveAction;
        PlayerInput.Instance.OnSelectAction += PlayerInput_OnSelectAction;
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
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
