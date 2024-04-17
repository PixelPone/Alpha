using System.Collections.Generic;
using UnityEngine;
using static Constants;

/// <summary>
/// The current stats of an Entity currently in battle.
/// </summary>
public class BattleEntityStats : MonoBehaviour
{
    /// <summary>
    /// The base values that this BattleEntity has.
    /// </summary>
    /// <remarks>
    /// This is very useful for enemies that share a pool of stats.
    /// This can also be used for the player characters as well, who each would
    /// just have their own BaseStats ScriptableObject.
    /// </remarks>
    [SerializeField] private BattleEntityBaseStats baseStats;
    [SerializeField] private State defaultState;

    public Dictionary<Keys_Stats, int> CurrentStats { get; set; }

    public Dictionary<Keys_Stats, List<int>> addModifiers;
    public Dictionary<Keys_Stats, List<int>> multiplyModifiers;

    /// <summary>
    /// The default state that are first run when it is this BattleEntity's turn
    /// in battle.
    /// </summary>
    public State DefaultState { get { return defaultState; } }
    /// <summary>
    /// Current battle grid position of this BattleEntity.
    /// </summary>
    public Vector2Int BattleGridPosition { get; set; }

    private void Awake()
    {
        BattleGridPosition = new Vector2Int(4, 4);
        CurrentStats = new Dictionary<Keys_Stats, int> 
        {
            {Keys_Stats.KEY_CURRENT_HEALTH, baseStats.GetBaseStat(Keys_Stats.KEY_MAX_HEALTH)},
            {Keys_Stats.KEY_MAX_HEALTH, baseStats.GetBaseStat(Keys_Stats.KEY_MAX_HEALTH)},
            {Keys_Stats.KEY_CURRENT_AP, baseStats.GetBaseStat(Keys_Stats.KEY_MAX_AP)},
            {Keys_Stats.KEY_MAX_AP, baseStats.GetBaseStat(Keys_Stats.KEY_MAX_AP)},
            {Keys_Stats.KEY_ATTACK, baseStats.GetBaseStat(Keys_Stats.KEY_ATTACK)},
            {Keys_Stats.KEY_DEFENSE, baseStats.GetBaseStat(Keys_Stats.KEY_DEFENSE)},
            {Keys_Stats.KEY_SPEED, baseStats.GetBaseStat(Keys_Stats.KEY_SPEED)}
        };
        addModifiers = new Dictionary<Keys_Stats, List<int>>
        {
            { Keys_Stats.KEY_MAX_HEALTH, new List<int>() },
            { Keys_Stats.KEY_CURRENT_HEALTH, new List<int>() },
            { Keys_Stats.KEY_MAX_AP, new List<int>() },
            { Keys_Stats.KEY_CURRENT_AP, new List<int>() },
            { Keys_Stats.KEY_ATTACK, new List<int>() },
            { Keys_Stats.KEY_DEFENSE, new List<int>() },
            { Keys_Stats.KEY_SPEED, new List<int>() }
        };
        multiplyModifiers = new Dictionary<Keys_Stats, List<int>>
        {
            { Keys_Stats.KEY_MAX_HEALTH, new List<int>() },
            { Keys_Stats.KEY_CURRENT_HEALTH, new List<int>() },
            { Keys_Stats.KEY_MAX_AP, new List<int>() },
            { Keys_Stats.KEY_CURRENT_AP, new List<int>() },
            { Keys_Stats.KEY_ATTACK, new List<int>() },
            { Keys_Stats.KEY_DEFENSE, new List<int>() },
            { Keys_Stats.KEY_SPEED, new List<int>() }
        };
    }

    public int GetStat(Keys_Stats key)
    {
        int startValue;
        int addValue = 0;
        int multValue = 1;
        if(key == Keys_Stats.KEY_CURRENT_HEALTH || key == Keys_Stats.KEY_CURRENT_AP)
        {
            startValue = CurrentStats[key];
        }
        else
        {
            startValue = baseStats.GetBaseStat(key);
        }

        foreach(int value in addModifiers[key])
        {
            addValue += value;
        }

        foreach(int value in multiplyModifiers[key])
        {
            multValue += value;
        }

        return (startValue + addValue) * multValue;
    }
}
