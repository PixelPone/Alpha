
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Internal logic for PlayerMoveSelection CombatState
/// </summary>
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
    private List<Vector2Int> selectMovements;

    private ActorStats actorStats;
    private BattleGrid battleGrid;

    public MoveStateHelper(ActorStats actorStats, BattleGrid battleGrid)
    {
        costOfCurrentPath = 0;
        selectMovements = new List<Vector2Int>();

        this.actorStats = actorStats;
        this.battleGrid = battleGrid;
        startOfCurrentPath = actorStats.BattleGridPosition;
        centerPosition = startOfCurrentPath;
        hoverPosition = startOfCurrentPath;
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
    }
}
