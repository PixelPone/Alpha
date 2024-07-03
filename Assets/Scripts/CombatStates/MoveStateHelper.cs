
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Internal logic for PlayerMoveSelection CombatState
/// </summary>
[System.Serializable]
public class MoveStateHelper
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

    private ActorStats actorStats;
    private BattleGrid battleGrid;

    public MoveStateHelper(ActorStats actorStats, BattleGrid battleGrid)
    {
        costOfCurrentPath = 0;
        SelectMovements = new List<Vector2Int>();

        this.actorStats = actorStats;
        this.battleGrid = battleGrid;

        startOfCurrentPath = actorStats.BattleGridPosition;
        centerPosition = startOfCurrentPath;
        hoverPosition = startOfCurrentPath;
    }

    private int GetCostOfPathMovement(Vector2Int movement)
    {
        return movement.x != 0 && movement.y != 0 ? 2 : 1;
    }

    public void UpdateHoverPosition(Vector2 actorInput)
    {
        Vector2Int potentialNewPosition = hoverPosition + Vector2Int.RoundToInt(actorInput);

        Debug.Log($"{actorStats.name} Input: {actorInput}");
        Debug.Log($"{actorStats.name} Input: {Vector2Int.RoundToInt(actorInput)}");
        Debug.Log($"Potential New Hover Position: {potentialNewPosition}");

        if (battleGrid.IsGridPositionInBounds(potentialNewPosition))
        {
            hoverPosition = potentialNewPosition;
        }

        Debug.Log($"Updated Hover Position: {hoverPosition}");
    }

    public (bool, string) AddMovementToStack()
    {
        Vector2Int movement = hoverPosition - centerPosition;
        int costOfMovement = GetCostOfPathMovement(movement);

        Debug.Log($"Position Difference: {movement}");
        SelectMovements.Add(movement);
        //Add cost of movement that was just added to path
        costOfCurrentPath += costOfMovement;
        Debug.Log("Position Movement Added!");
        centerPosition = hoverPosition;
        //UpdateBounds(centerPosition);

        return (true, "NO ERROR!");
    }
}
