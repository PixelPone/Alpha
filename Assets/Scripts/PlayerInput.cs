using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public event EventHandler<InputActionArgs> OnMoveAction;
    public event EventHandler<InputActionArgs> OnAcceptAction;

    public class InputActionArgs : EventArgs
    {
        public InputAction.CallbackContext callbackContext;
    }

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Battle.Enable();
        inputActions.Battle.Move.performed += Move_performed;
        inputActions.Battle.Accept.performed += Accept_performed;
    }

    private void Accept_performed(InputAction.CallbackContext obj)
    {
        OnAcceptAction?.Invoke(this, new InputActionArgs() { callbackContext = obj });
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        OnMoveAction?.Invoke(this, new InputActionArgs() { callbackContext = obj });
    }
}
