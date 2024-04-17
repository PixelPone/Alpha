using System.Collections.Generic;
using UnityEngine;
using static Constants;

/// <summary>
/// The move selection phase of the player's battle turn.
/// </summary>
public class WeaponAttackState : State
{
    /// <summary>
    /// The AP cost of attack.
    /// </summary>
    [SerializeField] private int costOfAttack;
    /// <summary>
    /// Index of center of area that player can select from.
    /// </summary>
    private Vector2Int centerPosition;
    /// <summary>
    /// Current index of grid that player is currently hovering over.
    /// </summary>
    private Vector2Int hoverPosition;
    /// <summary>
    /// Bounds in which the player can select each tile of the path they
    /// want to move.
    /// </summary>
    [SerializeReference, SubclassSelector] private SelectionBase selectionBounds;

    [SerializeField] private BattleEntityStats battleStats;
    private BattleGrid battleGrid;

    public void Awake()
    {
        hoverPosition = Vector2Int.zero;
    }

    private void Start()
    {
        battleGrid = BattleManager.Instance.BattleGridProperty;
        //Need to abstract this for any weapon type
        if (transform.parent.GetComponent<Pistol>() != null)
        {
            battleStats = transform.parent.GetComponent<Pistol>().User;
        }
    }

    public override void StartState()
    {
        Debug.Log("WeaponAttackState's StartState Ran!");
        
        centerPosition = battleStats.BattleGridPosition;
        hoverPosition = centerPosition;

        //selectionBounds = new CenterSquareSelection(new Vector2Int(centerPosition.x, centerPosition.y), 1);
        selectionBounds.UpdateSelectionArea(centerPosition);

        BattleManager.Instance.playerInput.OnMoveAction += PlayerInput_OnMoveAction;
        BattleManager.Instance.playerInput.OnSelectAction += PlayerInput_OnSelectAction;
        BattleManager.Instance.playerInput.OnAltSelectAction += PlayerInput_OnAltSelectAction;
    }

    public override void UpdateState(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public override void EndState()
    {
        Debug.Log("WeaponAttackState's EndState Ran!");
        BattleManager.Instance.playerInput.OnSelectAction -= PlayerInput_OnSelectAction;
        BattleManager.Instance.playerInput.OnAltSelectAction -= PlayerInput_OnAltSelectAction;
    }

    private void PlayerInput_OnMoveAction(object sender, PlayerInput.InputActionArgs args)
    {
        Vector2 playerInput = args.callbackContext.ReadValue<Vector2>();

        if (playerInput == Vector2Int.left || playerInput == Vector2Int.right
            || playerInput == Vector2Int.up || playerInput == Vector2Int.down)
        {
            Vector2Int newHoverPosition = hoverPosition + Vector2Int.RoundToInt(playerInput);
            //Debug.Log("Player Input: " + playerInput);
            //Debug.Log("Rounded Player Input: " + Vector2Int.RoundToInt(playerInput));
            //Debug.Log("Potential New Hover Position: " + newHoverPosition);

            //Have to check if the potential movement is in bounds or not
            //As a result, need to check -1 rather then 0
            if (selectionBounds.SelectionArea.Contains(newHoverPosition)
                && newHoverPosition.x < battleGrid.Width && newHoverPosition.y < battleGrid.Height
                && newHoverPosition.x > -1 && newHoverPosition.y > -1)
            {
                hoverPosition = newHoverPosition;
            }

        }
    }

    private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs args)
    {

        //Player selects a tile that is not themselves and has enough AP to attack
        if (!centerPosition.Equals(hoverPosition) && costOfAttack <= battleStats.GetStat(Keys_Stats.KEY_CURRENT_AP))
        {
            battleStats.CurrentStats[Keys_Stats.KEY_CURRENT_AP] -= costOfAttack;
            BattleManager.Instance.NextState();
        }
        else
        {
            //Player does not have enough AP at all to attack
            string feedback = string.Empty;
            if (battleStats.GetStat(Keys_Stats.KEY_CURRENT_AP) < costOfAttack)
            {
                feedback = "You do not have enough Action Points to attack!";
            }
            //Player selects a tile that is themselves and has enough AP to attack
            else if (battleStats.GetStat(Keys_Stats.KEY_CURRENT_AP) > 0 && centerPosition.Equals(centerPosition))
            {
                feedback = "You can't attack yourself!";
            }
            Debug.Log(feedback);
        }
    }

    private void PlayerInput_OnAltSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        BattleManager.Instance.PreviousState();
    }
}
