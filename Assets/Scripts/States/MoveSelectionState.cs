using System.Collections.Generic;
using UnityEngine;
using static Constants;

/// <summary>
/// The move selection phase of the player's battle turn.
/// </summary>
public class MoveSelectionState : State
{
    /// <summary>
    /// The AP cost of the current potential path being built.
    /// </summary>
    private int costOfCurrentPath;
    /// <summary>
    /// The start of the current potential path being built.
    /// </summary>
    private Vector2Int startOfCurrentPath;
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

    /// <summary>
    /// List that stores the path that the player makes- used for undo as well.
    /// </summary>
    /// <remarks>
    /// I used a List instead of a Stack in order to iterate through the path the
    /// player creates.
    /// </remarks>
    private List<Vector2Int> selectMovements;

    [SerializeField] private BattleEntityStats battleEntityStats;
    private BattleGrid battleGrid;

    public void Awake()
    {
        costOfCurrentPath = 0;
        hoverPosition = Vector2Int.zero;
        selectMovements = new List<Vector2Int>();
    }

    private void Start()
    {
        battleGrid = BattleManager.Instance.BattleGridProperty;
    }

    public override void StartState()
    {
        Debug.Log("MoveSelectionState's StartState Ran!");

        centerPosition = battleEntityStats.BattleGridPosition;
        startOfCurrentPath = centerPosition;
        hoverPosition = centerPosition;

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
        Debug.Log("MoveSelectionState's EndState Ran!");
        BattleManager.Instance.playerInput.OnSelectAction -= PlayerInput_OnSelectAction;
        BattleManager.Instance.playerInput.OnAltSelectAction -= PlayerInput_OnAltSelectAction;
    }

    private int GetCostOfPathMovement(Vector2Int movement)
    {
        return movement.x != 0 && movement.y != 0 ? 2 : 1;
    }

    /// <summary>
    /// Updates selection bounds from which a player can currently select a tile
    /// to move to.
    /// </summary>
    /// <param name="center">Center index who the new bounds are to be based on.</param>
    private void UpdateBounds(Vector2Int center)
    {
        /*selectionBounds = new CenterSquareSelection(new Vector2Int(center.x - halfWidth, center.y - halfHeight),
            selectionWidth, selectionHeight);*/
        selectionBounds.UpdateSelectionArea(centerPosition);
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
        //Debug.Log("SelectionAction Ran in MoveSelection!");
        //Debug.Log($"The value of the square you are touching is: {battleGrid.GetSquareValue(hoverPosition.x, hoverPosition.y)}");
        Vector2Int movement = hoverPosition - centerPosition;
        int costOfMovement = GetCostOfPathMovement(movement);
        
        //Player is trying to add another battle tile to the path they are building and is able to
        if (!centerPosition.Equals(hoverPosition) && (costOfCurrentPath + costOfMovement) 
            <= battleEntityStats.GetStat(Keys_Stats.KEY_CURRENT_AP))
        {
            //Debug.Log($"Position Difference: {movement}");
            selectMovements.Add(movement);
            //Add cost of movement that was just added to path
            costOfCurrentPath += costOfMovement;
            //Debug.Log("Position Movement Added!");
            centerPosition = hoverPosition;
            UpdateBounds(centerPosition);

            //Update green squares showing path so far to account for this new movement
            Vector2Int currentPath = startOfCurrentPath;
            foreach (Vector2Int position in selectMovements)
            {
                currentPath += position;
                //battleGrid.DrawSquare(Color.green, currentPath.x, currentPath.y);
            }
        }
        //Player selects the center position, ending the path building process
        else if (centerPosition.Equals(hoverPosition) && selectMovements.Count != 0)
        {
            battleEntityStats.CurrentStats[Keys_Stats.KEY_CURRENT_AP] -= costOfCurrentPath;
            BattleManager.Instance.NextState();
            //StartCoroutine(MoveAlongPathCoroutine());
        }
        //At this point, player is either trying to extend past what they can move to or
        //hasn't created a path at all
        else
        {
            string feedback = string.Empty;
            if(battleEntityStats.GetStat(Keys_Stats.KEY_CURRENT_AP) == 0)
            {
                feedback = "You do not have enough Action Points to create a path!";
            }
            else if(battleEntityStats.GetStat(Keys_Stats.KEY_CURRENT_AP) > 0 && selectMovements.Count == 0)
            {
                feedback = "You haven't created a Path to move yet!";
            }
            else if(battleEntityStats.GetStat(Keys_Stats.KEY_CURRENT_AP) > 0 && selectMovements.Count > 0)
            {
                feedback = "You do not have enough Action Points to move this far!";
            }

            Debug.Log(feedback);
            //StartCoroutine(DialogPathErrorCoroutine(feedback));
        }
    }

    private void PlayerInput_OnAltSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        //Debug.Log("AltSelectionAction Ran in MoveSelection!");
        if (selectMovements.Count > 0)
        {
            Vector2Int top = selectMovements[selectMovements.Count - 1];
            selectMovements.RemoveAt(selectMovements.Count - 1);
            Vector2Int reverse = new Vector2Int(-top.x, -top.y);
            int costOfReverse = GetCostOfPathMovement(reverse);
            costOfCurrentPath -= costOfReverse;
            centerPosition += reverse;
            UpdateBounds(centerPosition);

            //Given the grid was just refreshed, redraw current path up to the this new final step
            Vector2Int currentPath = startOfCurrentPath;
            foreach (Vector2Int position in selectMovements)
            {
                currentPath += position;
                //battleGrid.DrawSquare(Color.green, currentPath.x, currentPath.y);
            }
            hoverPosition = centerPosition;

        }
        else
        {
            BattleManager.Instance.PreviousState();
        }

    }
}
