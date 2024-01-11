using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectionState : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("MoveSelectionState's OnEnable Ran!");
        BattleManager.Instance.playerInput.OnSelectAction += PlayerInput_OnSelectAction;
    }

    private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs e)
    {
        Debug.Log("This is running in MoveSelection!");
    }
}
