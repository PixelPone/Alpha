using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The instance of a BattleEntity currently in battle.
/// </summary>
public class BattleEntity : MonoBehaviour
{
    /// <summary>
    /// The default values that this BattleEntity has.
    /// </summary>
    /// <remarks>
    /// This is very useful for enemies that share a pool of stats.
    /// </remarks>
    [SerializeField] private BattleEntityData battleEntityData;
    [SerializeField] private State defaultState;
    /// <summary>
    /// The default state that are first run when it is this BattleEntity's turn
    /// in battle.
    /// </summary>
    public State DefaultState { get { return defaultState; } }
    /// <summary>
    /// Current battle grid position of this BattleEntity.
    /// </summary>
    public Vector2Int BattleGridPosition { get; set; }

    /// <summary>
    /// The current AP that this BattleEntity has in battle.
    /// </summary>
    public int CurrentAP { get; set; }

    private void Awake()
    {
        CurrentAP = battleEntityData.MaxAP;
        BattleGridPosition = new Vector2Int(4, 4);
    }
}
