using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class ActorStats : MonoBehaviour
{
    //Where this Actor's General Information and Base SPECIAL stats are stored
    [SerializeField]
    private BaseActorStats baseActorStats;

    private int level = 1;
    private int experience = 0;
    private int karma;
    private int size;

    //Actor's SPECIAL stats
    //Ranges [1 - 10] normally, can go up to 15 with temporary buffs
    //This is so that each indiviual Actor can have their own SPECIAL if needed
    //(useful for permenant modifiers)
    public int Strength { get; private set; }
    public int Perception { get; private set; }
    public int Endurance { get; private set; }
    public int Charisma { get; private set; }
    public int Intelligence {  get; private set; }
    public int Agility { get; private set; }
    public int Luck { get; private set; }

    private int currentHp;
    private int currentAp;
    private int currentStrain;
    private int currentInsanity;

    public int CurrentHp { get { return currentHp; } set { currentHp = Mathf.Clamp(value, 0, MaxHp); } }
    public int CurrentAp { get { return currentAp; } set { currentAp = Mathf.Clamp(value, 0, MaxAp); } }
    public int CurrentStrain { get { return currentStrain; } set { currentStrain = Mathf.Clamp(value, 0, MaxStrain); } }
    public int CurrentInsanity { get { return currentInsanity; } set { currentInsanity = Mathf.Clamp(value, 0, MaxInsanity); } }

    //Secondary Stats
    //Pain Thresholds at 5 HP, 3 HP, and 1 HP
    public int MaxHp { get; private set; }
    public int MaxAp { get; private set; }
    //Default = 20, +5 every 5 levels
    public int MaxStrain { get; private set; }
    public int MaxInsanity { get; private set; }

    public int HealingRate { get; private set; }
    public int SkillPoints { get; private set; }
    public int CarryWeight { get; private set; }

    //Resistances
    public int ResistancePoision {  get; private set; }
    public int ResistanceRadiation { get; private set; }
    //Used for SPECIAL Check
    public int ResistanceCold { get; private set; }
    //Used for Endurance SPECIAL Check
    public int ResistanceHeat { get; private set; }
    //Used for SPECIAL Check
    public int ResistanceElectricity { get; private set; }

    //Skills
    public int SkillBarter { get; private set; }
    public int SkillDiplomacy {  get; private set; }
    public int SkillExplosives { get; private set; }
    public int SkillFirearms { get; private set; }
    public int SkillIntimidation { get; private set; }
    public int SkillLockpick { get; private set; }
    public int SkillMagicEnergyWeapons { get; private set; }
    public int SkillMechanics { get; private set; }
    public int SkillMedicine { get; private set; }
    public int SkillMelee { get; private set; }
    public int SkillScience { get; private set; }
    public int SkillSleight { get; private set; }
    public int SkillSneak { get; private set; }
    public int SkillSurvival { get; private set; }
    public int SkillThaumaturgy { get; private set; }
    public int SkillUnarmed { get; private set; }

    private void Awake()
    {
        Strength = baseActorStats.Strength;
        Perception = baseActorStats.Perception;
        Endurance = baseActorStats.Endurance;
        Charisma = baseActorStats.Charisma;
        Intelligence = baseActorStats.Intelligence;
        Agility = baseActorStats.Agility;
        Luck = baseActorStats.Luck;

        UpdateAllSecondaryStats();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void UpdateAllSecondaryStats()
    {
        MaxHp = 10 + Endurance;
        MaxAp = 10 + (Agility / 2);
        int strainBonus = (level / 5);
        MaxStrain = 20 + (strainBonus * 5);
        MaxInsanity = 5 + (Intelligence / 2);

        CurrentHp = MaxHp;
        CurrentAp = MaxAp;
        CurrentStrain = MaxStrain;
        CurrentInsanity = 0;

        if (Endurance >= 1 && Endurance <= 3)
        {
            HealingRate = 1;
        }
        else if (Endurance >= 3 && Endurance <= 7)
        {
            HealingRate = 2;
        }
        else
        {
            HealingRate = 3;
        }

        SkillPoints = 10 + (Intelligence / 2);
        CarryWeight = 10 + (Strength / 2);

        ResistancePoision = 10;
        ResistanceRadiation = 5;
        ResistanceCold = Endurance / Agility;
        ResistanceHeat = Endurance;
        ResistanceElectricity = Endurance / Strength;

        //Skills
        SkillBarter = (2 * Charisma) + (Luck / 2);
        SkillDiplomacy = (2 * Charisma) + (Luck / 2);
        SkillExplosives = (2 * Perception) + (Luck / 2);
        SkillFirearms = (2 * Agility) + (Luck / 2);
        SkillIntimidation = (2 * Strength) + (Luck / 2);
        SkillLockpick = (2 * Perception) + (Luck / 2);
        SkillMagicEnergyWeapons = (2 * Perception) + (Luck / 2);
        SkillMechanics = (2 * Intelligence) + (Luck / 2);
        SkillMelee = (2 * Strength) + (Luck / 2);
        SkillScience = (2 * Intelligence) + (Luck / 2);
        SkillSleight = (2 * Agility) + (Luck / 2);
        SkillSneak = (2 * Agility) + (Luck / 2);
        SkillSurvival = (2 * Endurance) + (Luck / 2);
        SkillThaumaturgy = (2 * baseActorStats.MagicSpecialValue) + (Luck / 2);
        SkillUnarmed = (2 * Endurance) + (Luck / 2);

    }
}
