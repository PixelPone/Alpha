using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "BaseActorStats", menuName = "Alpha/BaseActorStats")]
public class BaseActorStats : ScriptableObject
{
    [SerializeField, TextArea]
    private string description;
    [SerializeField]
    private string raceName = "Earth";

    //SPECIAL stats
    //Ranges [1 - 10] normally, can go up to 15 with temporary buffs
    [SerializeField, Range(1, 10)]
    private int strength = 5;
    [SerializeField, Range(1, 10)]
    private int perception = 5;
    [SerializeField, Range(1, 10)]
    private int endurance = 5;
    [SerializeField, Range(1, 10)]
    private int charisma = 5;
    [SerializeField, Range(1, 10)]
    private int intelligence = 5;
    [SerializeField, Range(1, 10)]
    private int agility = 5;
    [SerializeField, Range(1, 10)]
    private int luck = 5;

    [SerializeField]
    private Special_Name magicSpecial;
    private int magicSpecialValue;

    //Secondary Stats
    //Pain Thresholds at 5 HP, 3 HP, and 1 HP
    private int maxHp;
    private int maxAp;

    private int healingRate;
    //Default = 20, +5 every 5 levels
    private int maxStrain;
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
        Debug.Log("On BaseActorStat Awake!");
        Debug.Log($"strength {strength}");
        Debug.Log($"perception {perception}");
        Debug.Log($"endurance {endurance}");
        Debug.Log($"charisma {charisma}");
        Debug.Log($"intelligence {intelligence}");
        Debug.Log($"agility {agility}");
        Debug.Log($"luck {luck}");

        UpdateAllSecondaryStats();
    }

    public void OnValidate()
    {
        /*Debug.Log("On BaseActorStat OnValidate!");
        Debug.Log($"strength {strength}");
        Debug.Log($"perception {perception}");
        Debug.Log($"endurance {endurance}");
        Debug.Log($"charisma {charisma}");
        Debug.Log($"intelligence {intelligence}");
        Debug.Log($"agility {agility}");
        Debug.Log($"luck {luck}");*/

        UpdateAllSecondaryStats();
    }

    public void Reset()
    {
        strength = 5;
        perception = 5;
        endurance = 5;
        charisma = 5;
        intelligence = 5;
        agility = 5;
        luck = 5;

        /*Debug.Log("On BaseActorStat Reset!");
        Debug.Log($"strength {strength}");
        Debug.Log($"perception {perception}");
        Debug.Log($"endurance {endurance}");
        Debug.Log($"charisma {charisma}");
        Debug.Log($"intelligence {intelligence}");
        Debug.Log($"agility {agility}");
        Debug.Log($"luck {luck}");*/

        UpdateAllSecondaryStats();
    }

    private void UpdateAllSecondaryStats()
    {
        maxHp = 10 + endurance;
        maxAp = 10 + (agility / 2);

        if (endurance >= 1 && endurance <= 3)
        {
            healingRate = 1;
        }
        else if (endurance >= 3 && endurance <= 7)
        {
            healingRate = 2;
        }
        else
        {
            healingRate = 3;
        }

        maxInsanity = 5 + (intelligence / 2);

        maxStrain = 20;

        skillPoints = 10 + (intelligence / 2);
        carryWeight = 10 + (strength / 2);

        poisionResistance = 1 / 10;
        radiationResistance = 1 / 20;
        coldResistance = endurance / agility;
        heatResistance = endurance;
        electricityResistance = endurance / strength;

        switch (magicSpecial)
        {
            case Special_Name.STRENGTH:
                magicSpecialValue = strength;
                break;
            case Special_Name.PERCEPTION:
                magicSpecialValue = perception;
                break;
            case Special_Name.ENDURANCE:
                magicSpecialValue = endurance;
                break;
            case Special_Name.CHARISMA:
                magicSpecialValue = charisma;
                break;
            case Special_Name.INTELLIGENCE:
                magicSpecialValue = intelligence;
                break;
            case Special_Name.AGILITY:
                magicSpecialValue = agility;
                break;
            case Special_Name.LUCK:
                Debug.LogWarning("Magic Special is set to LUCK. This is not a valid SPECIAL!");
                break;
            default:
                magicSpecialValue = strength;
                break;
        }


        //Skills
        barter = (2 * charisma) + (luck / 2);
        diplomacy = (2 * charisma) + (luck / 2);
        explosives = (2 * perception) + (luck / 2);
        firearms = (2 * agility) + (luck / 2);
        intimidation = (2 * strength) + (luck / 2);
        lockpick = (2 * perception) + (luck / 2);
        magicEnergyWeapons = (2 * perception) + (luck / 2);
        mechanics = (2 * intelligence) + (luck / 2);
        melee = (2 * strength) + (luck / 2);
        science = (2 * intelligence) + (luck / 2);
        sleight = (2 * agility) + (luck / 2);
        sneak = (2 * agility) + (luck / 2);
        survival = (2 * endurance) + (luck / 2);
        thaumaturgy = (2 * magicSpecialValue) + (luck / 2);
        unarmed = (2 * endurance) + (luck / 2);
    }
}
