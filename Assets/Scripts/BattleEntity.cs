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
    /// This is very useful for Enemies that share a pool of stats.
    /// </remarks>
    [SerializeField] private BattleEntityData battleEntityData;
    [SerializeField] private GameObject defaultBehavior;
    /// <summary>
    /// The default state(s) that are first run when it is this BattleEntity's turn
    /// in battle.
    /// </summary>
    public GameObject DefaultBehavior { get { return defaultBehavior; } }
    public Vector2 BattleGridPosition { get; set; }
}
