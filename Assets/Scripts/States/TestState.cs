using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : MonoBehaviour
{
    private void OnEnable()
    {
        BattleManager.Instance.playerInput.OnSelectAction += PlayerInput_OnSelectAction;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        Debug.Log("This is from Test State!");
    }

    private void OnDisable()
    {
        BattleManager.Instance.playerInput.OnSelectAction -= PlayerInput_OnSelectAction;
    }
}
