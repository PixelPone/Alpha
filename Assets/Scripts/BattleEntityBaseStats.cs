using System.Collections.Generic;
using UnityEngine;
using static Constants;

/// <summary>
/// Stores the main information about a battle entity (player, enemies, etc).
/// </summary>
[CreateAssetMenu(fileName = "BattleEntityBaseStats", menuName = "Alpha/BattleEntityBaseStats")]
public class BattleEntityBaseStats : ScriptableObject
{
    [SerializeField][TextArea] private string description;
    [SerializeField] private int maxHP;
    [SerializeField] private int maxAP;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int speed;

    private Dictionary<Keys_Stats, int> baseStats;

    public void OnEnable()
    {
        baseStats = new Dictionary<Keys_Stats, int>
        {
            { Keys_Stats.KEY_MAX_HEALTH, maxHP },
            { Keys_Stats.KEY_MAX_AP, maxAP },
            { Keys_Stats.KEY_ATTACK, attack },
            { Keys_Stats.KEY_DEFENSE, defense },
            { Keys_Stats.KEY_SPEED, speed }
        };
    }

    public void OnValidate()
    {
        //Normally don't want to change the values that are stored in ScriptableObject
        //But this is here for testing in editor
        //This is so that when any of these values get changed in the editor, the dictionary is updated
        //to account for those changes.
        baseStats = new Dictionary<Keys_Stats, int>
        {
            { Keys_Stats.KEY_MAX_HEALTH, maxHP },
            { Keys_Stats.KEY_MAX_AP, maxAP },
            { Keys_Stats.KEY_ATTACK, attack },
            { Keys_Stats.KEY_DEFENSE, defense },
            { Keys_Stats.KEY_SPEED, speed }
        };
    }

    public int GetBaseStat(Keys_Stats statKey)
    {
        return baseStats[statKey];
    }

}