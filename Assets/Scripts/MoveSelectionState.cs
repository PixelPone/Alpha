using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

/// <summary>
/// The move selection phase of the player's battle turn.
/// </summary>
public class MoveSelectionState : MonoBehaviour
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
    /// Index of top left box of area that player can select from.
    /// </summary>
    private Vector2Int selectBoundsTopLeft;
    /// <summary>
    /// Index of bottom right box of area that player can select from.
    /// </summary>
    private Vector2Int selectBoundsBotRight;
    /// <summary>
    /// List that stores the path that the player makes- used for undo as well.
    /// </summary>
    /// <remarks>
    /// I used a List instead of a Stack in order to iterate through the path the
    /// player creates.
    /// </remarks>
    private List<Vector2Int> selectMovements;

    [SerializeField] private BattleEntity battleEntity;
    private BattleGrid battleGrid;

    public void Awake()
    {
        costOfCurrentPath = 0;
        hoverPosition = Vector2Int.zero;
        selectBoundsTopLeft = Vector2Int.zero;
        selectBoundsBotRight = Vector2Int.zero;
        selectMovements = new List<Vector2Int>();
    }

    private void OnEnable()
    {
        Debug.Log("MoveSelectionState's OnEnable Ran!");

        centerPosition = battleEntity.BattleGridPosition;
        startOfCurrentPath = centerPosition;
        hoverPosition = centerPosition;

        BattleManager.Instance.playerInput.OnMoveAction += PlayerInput_OnMoveAction;
        BattleManager.Instance.playerInput.OnSelectAction += PlayerInput_OnSelectAction;
        BattleManager.Instance.playerInput.OnAltSelectAction += PlayerInput_OnAltSelectAction;
    }

    private void Start()
    {
        battleGrid = BattleManager.Instance.BattleGridProperty;
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

        selectBoundsTopLeft = new Vector2Int(center.x - 1, center.y + 1);
        selectBoundsBotRight = new Vector2Int(center.x + 1, center.y - 1);
        if (selectBoundsTopLeft.x < 0)
        {
            selectBoundsTopLeft.x = 0;
        }
        if (selectBoundsTopLeft.y > battleGrid.Height - 1)
        {
            selectBoundsTopLeft.y = battleGrid.Height - 1;
        }
        if (selectBoundsBotRight.x > battleGrid.Width - 1)
        {
            selectBoundsBotRight.x = battleGrid.Width - 1;
        }
        if (selectBoundsBotRight.y < 0)
        {
            selectBoundsBotRight.y = 0;
        }
    }

        private void PlayerInput_OnMoveAction(object sender, PlayerInput.InputActionArgs args)
    {
        Vector2 playerInput = args.callbackContext.ReadValue<Vector2>();
        //Debug.Log("Player Input: " + playerInput);

        //Bound player input to selection square
        if (playerInput.Equals(Vector2Int.left) && hoverPosition.x > 0
            && hoverPosition.x > centerPosition.x - 1)
        {
            hoverPosition.x--;
        }
        else if (playerInput.Equals(Vector2Int.right) && hoverPosition.x < battleGrid.Width - 1
            && hoverPosition.x < centerPosition.x + 1)
        {
            hoverPosition.x++;
        }
        else if (playerInput.Equals(Vector2Int.down) && hoverPosition.y > 0
            && hoverPosition.y > centerPosition.y - 1)
        {
            hoverPosition.y--;
        }
        else if (playerInput.Equals(Vector2Int.up) && hoverPosition.y < battleGrid.Height - 1
            && hoverPosition.y < centerPosition.y + 1)
        {
            hoverPosition.y++;
        }
    }

    private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        Debug.Log("SelectionAction Ran in MoveSelection!");
        Debug.Log($"The value of the square you are touching is: {battleGrid.GetSquareValue(hoverPosition.x, hoverPosition.y)}");
        Vector2Int movement = hoverPosition - centerPosition;
        int costOfMovement = GetCostOfPathMovement(movement);
        if (!centerPosition.Equals(hoverPosition) && (costOfCurrentPath + costOfMovement) <= battleEntity.CurrentAP)
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
                battleGrid.DrawSquare(Color.green, currentPath.x, currentPath.y);
            }
        }
        else if (centerPosition.Equals(hoverPosition) && selectMovements.Count != 0)
        {
            battleEntity.CurrentAP -= costOfCurrentPath;
            //StartCoroutine(MoveAlongPathCoroutine());
        }
        else
        {
            //At this point, player is either trying to extend past what they can move to or
            //hasn't created a path at all

            string feedback = selectMovements.Count != 0
                ? "You do not have enough Action Points to move this far!"
                : "You haven't created a Path to move yet!";
            //StartCoroutine(DialogPathErrorCoroutine(feedback));
        }
    }

    private void PlayerInput_OnAltSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        Debug.Log("AltSelectionAction Ran in MoveSelection!");
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
                battleGrid.DrawSquare(Color.green, currentPath.x, currentPath.y);
            }
            hoverPosition = centerPosition;

        }
        else
        {
            BattleManager.Instance.PreviousState();
        }
        
    }

    private void OnDisable()
    {
        Debug.Log("MoveSelectionState's OnDisable Ran!");
        BattleManager.Instance.playerInput.OnSelectAction -= PlayerInput_OnSelectAction;
        BattleManager.Instance.playerInput.OnAltSelectAction -= PlayerInput_OnAltSelectAction;
    }
}
