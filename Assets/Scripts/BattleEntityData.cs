using UnityEngine;

/// <summary>
/// Stores the main information about a battle entity (player, enemies, etc).
/// </summary>
[CreateAssetMenu(fileName = "BattleEntityData", menuName = "Alpha/BattleEntityData")]
public class BattleEntityData : ScriptableObject
{
    [SerializeField][TextArea] private string description;
    [SerializeField] private int maxHP;
    [SerializeField] private int maxAP;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int speed;

    public string Description { get { return description; } set { description = value; } }
    public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    public int MaxAP { get { return maxAP; } set { maxAP = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int Defense { get { return defense; } set { defense = value; } }
    public int Speed { get { return speed; } set { speed = value; } }

}