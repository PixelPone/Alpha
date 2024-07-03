using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class PlayerMoveSelection : CombatState
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
    /// Grid index of center of area that player can select from.
    /// </summary>
    private Vector2Int centerPosition;
    /// <summary>
    /// Current grid index of grid that player is currently hovering over.
    /// </summary>
    private Vector2Int hoverPosition;

    /// <summary>
    /// List that stores the path that the player makes- used for undo as well.
    /// </summary>
    /// <remarks>
    /// A List is instead of a Stack in order to iterate through the path that
    /// is created.
    /// </remarks>
    public List<Vector2Int> SelectMovements;

    private BattleGrid battleGrid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void StartState(BattleManager battleManager)
    {
        base.StartState(battleManager);

        costOfCurrentPath = 0;
        SelectMovements = new List<Vector2Int>();

        this.battleGrid = battleManager.BattleGridProperty;
        startOfCurrentPath = this.Owner.BattleGridPosition;
        centerPosition = startOfCurrentPath;
        hoverPosition = startOfCurrentPath;

        Debug.Log("MoveSelection's StartState method Ran!");
        PlayerInput.Instance.OnMoveAction += PlayerInput_OnMoveAction;
        PlayerInput.Instance.OnSelectAction += PlayerInput_OnSelectAction;
    }

    public override void UpdateState()
    {
        //throw new System.NotImplementedException();
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

    private int GetCostOfPathMovement(Vector2Int movement)
    {
        return movement.x != 0 && movement.y != 0 ? 2 : 1;
    }

    private void PlayerInput_OnMoveAction(object sender, PlayerInput.InputActionArgs args)
    {
        Vector2 playerInput = args.callbackContext.ReadValue<Vector2>();
        if (playerInput == Vector2Int.left || playerInput == Vector2Int.right
            || playerInput == Vector2Int.up || playerInput == Vector2Int.down)
        {
            Vector2Int potentialNewPosition = hoverPosition + Vector2Int.RoundToInt(playerInput);

            Debug.Log($"{this.Owner.name} Input: {playerInput}");
            Debug.Log($"{this.Owner.name} Input: {Vector2Int.RoundToInt(playerInput)}");
            Debug.Log($"Potential New Hover Position: {potentialNewPosition}");

            if (battleGrid.IsGridPositionInBounds(potentialNewPosition))
            {
                hoverPosition = potentialNewPosition;
            }

            Debug.Log($"Updated Hover Position: {hoverPosition}");
        }
    }

    private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs args)
    {
        Debug.Log("SelectionAction Ran in PlayerMoveSelection!");
        Debug.Log($"The value of the square you are touching is: {battleGrid.GetSquareValue(hoverPosition.x, hoverPosition.y)}");
        Vector2Int movement = hoverPosition - centerPosition;
        int costOfMovement = GetCostOfPathMovement(movement);

        //Player is trying to add another battle tile to the path they are building and is able to
        if (!centerPosition.Equals(hoverPosition) && (costOfCurrentPath + costOfMovement)
            <= this.Owner.CurrentAp)
        {
            Debug.Log($"Position Difference: {movement}");
            SelectMovements.Add(movement);
            //Add cost of movement that was just added to path
            costOfCurrentPath += costOfMovement;
            Debug.Log("Position Movement Added!");
            centerPosition = hoverPosition;
            //UpdateBounds(centerPosition);

            //Update green squares showing path so far to account for this new movement
            Vector2Int currentPath = startOfCurrentPath;
            foreach (Vector2Int position in SelectMovements)
            {
                currentPath += position;
                //battleGrid.DrawSquare(Color.green, currentPath.x, currentPath.y);
            }
        }
        //Player selects the center position, ending the path building process
        else if (centerPosition.Equals(hoverPosition) && SelectMovements.Count != 0)
        {
            this.Owner.CurrentAp -= costOfCurrentPath;
            battleManager.NextSubstate();
            //StartCoroutine(MoveAlongPathCoroutine());
        }
        //At this point, player is either trying to extend past what they can move to or
        //hasn't created a path at all
        else
        {
            string feedback = string.Empty;
            if (this.Owner.CurrentAp == 0)
            {
                feedback = "You do not have enough Action Points to create a path!";
            }
            else if (this.Owner.CurrentAp > 0 && SelectMovements.Count == 0)
            {
                feedback = "You haven't created a Path to move yet!";
            }
            else if (this.Owner.CurrentAp > 0 && SelectMovements.Count > 0)
            {
                feedback = "You do not have enough Action Points to move this far!";
            }

            Debug.Log(feedback);
            //StartCoroutine(DialogPathErrorCoroutine(feedback));
        }
    }
}
