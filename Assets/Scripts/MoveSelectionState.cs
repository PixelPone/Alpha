using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectionState : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("MoveSelectionState's OnEnable Ran!");
        BattleManager.Instance.playerInput.OnSelectAction += PlayerInput_OnSelectAction;
        BattleManager.Instance.playerInput.OnAltSelectAction += PlayerInput_OnAltSelectAction;
    }

    private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        Debug.Log("SelectionAction Ran in MoveSelection!");
    }

    private void PlayerInput_OnAltSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        Debug.Log("AltSelectionAction Ran in MoveSelection!");
        BattleManager.Instance.PreviousState();
    }

    private void OnDisable()
    {
        Debug.Log("MoveSelectionState's OnDisable Ran!");
        BattleManager.Instance.playerInput.OnSelectAction -= PlayerInput_OnSelectAction;
        BattleManager.Instance.playerInput.OnAltSelectAction -= PlayerInput_OnAltSelectAction;
    }
}
