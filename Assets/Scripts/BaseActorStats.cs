
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class BaseActorStats : ScriptableObject
{

    private string description;
    private string raceName = "Earth";

    //SPECIAL stats
    //Ranges [1 - 10] normally, can go up to 15 with temporary buffs
    [field: Range(1, 10)]
    [field: SerializeField]
    public int Strength { get; private set; }
    [field: Range(1, 10)]
    [field: SerializeField]
    public int Perception { get; private set; }
    [field: Range(1, 10)]
    [field: SerializeField]
    public int Endurance { get; private set; }
    [field: Range(1, 10)]
    [field: SerializeField]
    public int Charisma { get; private set; }
    [field: Range(1, 10)]
    [field: SerializeField]
    public int Intelligence { get; private set; }
    [field: Range(1, 10)]
    [field: SerializeField]
    public int Agility { get; private set; }
    [field: Range(1, 10)]
    [field: SerializeField]
    public int Luck { get; private set; }

    private int magicSpecial;

    //Secondary Stats
    //Pain Thresholds at 5 HP, 3 HP, and 1 HP
    private int currentHp;
    private int maxHp;
    private int currentAp;
    private int maxAp;

    private int healingRate;
    private int currentStrain;
    //Default = 20, +5 every 5 levels
    private int maxStrain;
    private int currentInsanity;
    private int maxInsanity;
    
    private int skillPoints;
    private int carryWeight;

    private int poisionResistance;
    private int radiationResistance;
    private int coldResistance; //SPECIAL Check
    private int heatResistance; //Endurance SPECIAL Check
    private int electricityResistance; //SPECIAL Check

    //Skills
    private int barter;
    private int diplomacy;
    private int explosives;
    private int firearms;
    private int intimidation;
    private int lockpick;
    private int magicEnergyWeapons;
    private int mechanics;
    private int medicine;
    private int melee;
    private int science;
    private int sleight;
    private int sneak;
    private int survival;
    private int thaumaturgy;
    private int unarmed;

    public void Awake()
    {
        maxHp = 10 + Endurance;
        maxAp = 10 + (Agility / 2);

        if (Endurance >= 1 && Endurance <= 3)
        {
            healingRate = 1;
        }
        else if (Endurance >= 3 && Endurance <= 7)
        {
            healingRate = 2;
        }
        else
        {
            healingRate = 3;
        }

        maxInsanity = 5 + (Intelligence / 2);
        currentInsanity = 0;

        maxStrain = 20;

        skillPoints = 10 + (Intelligence / 2);
        carryWeight = 10 + (Strength / 2);

        poisionResistance = 1 / 10;
        radiationResistance = 1 / 20;
        coldResistance = Endurance / Agility;
        heatResistance = Endurance;
        electricityResistance = Endurance / Strength;

        //Skills
        barter = (2 * Charisma) + (Luck / 2);
        diplomacy = (2 * Charisma) + (Luck / 2);
        explosives = (2 * Perception) + (Luck / 2);
        firearms = (2 * Agility) + (Luck / 2);
        intimidation = (2 * Strength) + (Luck / 2);
        lockpick = (2 * Perception) + (Luck / 2);
        magicEnergyWeapons = (2 * Perception) + (Luck / 2);
        mechanics = (2 * Intelligence) + (Luck / 2);
        melee = (2 * Strength) + (Luck / 2);
        science = (2 * Intelligence) + (Luck / 2);
        sleight = (2 * Agility) + (Luck / 2);
        sneak = (2 * Agility) + (Luck / 2);
        survival = (2 * Endurance) + (Luck / 2);
        thaumaturgy = (2 * magicSpecial) + (Luck / 2);
        unarmed = (2 * Endurance) + (Luck / 2);
    }
}
