using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages EventHandlers that are fired when input is pressed.
/// </summary>
/// <remarks>
/// Technically, by Unity's standards, the PlayerInputActions class is the one that
/// manages Actions that are fired when input is pressed using the New Input System.
/// However, as per Microsoft standard, one should use EventHandlers when dealing with
/// events. As a result, I invoked these EventHandlers when PlayerInputActions'
/// Actions run and then subscribed to these Handlers in other classes who needed
/// access to input.
/// <seealso cref="PlayerInputActions"/>
/// </remarks>
public class PlayerInput : MonoBehaviour
{
    public event EventHandler<InputActionArgs> OnMoveAction;
    public event EventHandler<InputActionArgs> OnSelectAction;

    /// <summary>
    /// Generic EventArgs class used to obtain callbackContext of PlayerInputActions'
    /// Actions- basically it's just an EventHandler version of those Actions.
    /// </summary>
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
        inputActions.Battle.Select.performed += Select_performed;
    }

    private void Select_performed(InputAction.CallbackContext obj)
    {
        OnSelectAction?.Invoke(this, new InputActionArgs() { callbackContext = obj });
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        OnMoveAction?.Invoke(this, new InputActionArgs() { callbackContext = obj });
    }
}
