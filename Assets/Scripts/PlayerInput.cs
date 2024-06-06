using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages EventHandlers that are fired when playerInput is pressed.
/// </summary>
/// <remarks>
/// Technically, by Unity's standards, the <seealso cref="PlayerInputActions"/> class is the one that
/// manages Actions that are fired when playerInput is pressed using the New Input System.
/// However, as per Microsoft standard, one should use EventHandlers when dealing with
/// events. As a result, I invoked these EventHandlers when PlayerInputActions'
/// Actions run and then subscribed to these Handlers in other classes who needed
/// access to playerInput.
/// </remarks>
public class PlayerInput : MonoBehaviour
{
    public event EventHandler<InputActionArgs> OnMoveAction;
    public event EventHandler<InputActionArgs> OnSelectAction;
    public event EventHandler<InputActionArgs> OnAltSelectAction;

    public static PlayerInput Instance { get; private set; }

    /// <summary>
    /// Generic EventArgs class used to obtain callbackContext of PlayerInputActions'
    /// Actions.
    /// </summary>
    public class InputActionArgs : EventArgs
    {
        public InputAction.CallbackContext callbackContext;
    }

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of BattleManager!", transform.gameObject);
        }
        Instance = this;

        inputActions.Battle.Enable();
        inputActions.Battle.Move.performed += Move_performed;
        inputActions.Battle.Select.performed += Select_performed;
        inputActions.Battle.AltSelect.performed += AltSelect_performed;
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        OnMoveAction?.Invoke(this, new InputActionArgs() { callbackContext = obj });
    }

    private void Select_performed(InputAction.CallbackContext obj)
    {
        OnSelectAction?.Invoke(this, new InputActionArgs() { callbackContext = obj });
    }

    private void AltSelect_performed(InputAction.CallbackContext obj)
    {
        OnAltSelectAction?.Invoke(this, new InputActionArgs() { callbackContext = obj });
    }
}
