using UnityEngine;
using Scripts.Perks;
using Scripts.CombatStates;

namespace Scripts.Actors
{
    /// <summary>
    /// Stores an in-game instances of a specific Actor's stats.
    /// </summary>
    public class ActorStats : MonoBehaviour
    {
        
        [field: SerializeField]
        public BaseActorStats BaseActorSpecial { get; private set; }
        [SerializeField]
        private ActorPast actorPast;

        //Any Battle Specific variables of an Actor- if there are too many, this will
        //be refractored into a separate component
        [field: SerializeField]
        public CombatState InitialCombatEvent { get; private set; }

        [field: SerializeField]
        public Vector2Int BattleGridPosition { get; set; }

        private int level = 1;
        private int experience = 0;
        private int karma;
        private int size;

        //Actor's SPECIAL stats
        //Ranges [1 - 10] normally, can go up to 15 with temporary buffs
        //This is so that each indiviual Actor can have their own SPECIAL if needed
        //(useful for permenant modifiers)
        [field: Header("SPECIAL Values"), SerializeField]
        public int Strength { get; set; }
        [field: SerializeField]
        public int Perception { get; set; }
        [field: SerializeField]
        public int Endurance { get; set; }
        [field: SerializeField]
        public int Charisma { get; set; }
        [field: SerializeField]
        public int Intelligence { get; set; }
        [field: SerializeField]
        public int Agility { get; set; }
        [field: SerializeField]
        public int Luck { get; set; }

        [Header("Current Values"), SerializeField]
        private int currentHp;
        [SerializeField]
        private int currentAp;
        [SerializeField]
        private int currentStrain;
        [SerializeField]
        private int currentInsanity;

        public int CurrentHp { get { return currentHp; } set { currentHp = Mathf.Clamp(value, 0, MaxHp); } }
        public int CurrentAp { get { return currentAp; } set { currentAp = Mathf.Clamp(value, 0, MaxAp); } }
        public int CurrentStrain { get { return currentStrain; } set { currentStrain = Mathf.Clamp(value, 0, MaxStrain); } }
        public int CurrentInsanity { get { return currentInsanity; } set { currentInsanity = Mathf.Clamp(value, 0, MaxInsanity); } }

        //Secondary Stats
        //Pain Thresholds at 5 HP, 3 HP, and 1 HP
        [field: Header("Max Thresholds"), SerializeField]
        public int MaxHp { get; private set; }
        [field: SerializeField]
        public int MaxAp { get; private set; }
        //Default = 20, +5 every 5 levels
        [field: SerializeField]
        public int MaxStrain { get; private set; }
        [field: SerializeField]
        public int MaxInsanity { get; private set; }

        [field: Header("Healing, Skills, and Weight"), SerializeField]
        public int HealingRate { get; private set; }
        [field: SerializeField]
        public int SkillPoints { get; private set; }
        [field: SerializeField]
        public int CarryWeight { get; private set; }

        //Resistances
        [field: Header("Resistances"), SerializeField]
        public int ResistancePoision { get; private set; }
        [field: SerializeField]
        public int ResistanceRadiation { get; private set; }
        //Used for SPECIAL Check
        [field: SerializeField]
        public int ResistanceCold { get; private set; }
        //Used for Endurance SPECIAL Check
        [field: SerializeField]
        public int ResistanceHeat { get; private set; }
        //Used for SPECIAL Check
        [field: SerializeField]
        public int ResistanceElectricity { get; private set; }

        //Skills
        [field: Header("Skills"), SerializeField]
        public int SkillBarter { get; set; }
        [field: SerializeField]
        public int SkillDiplomacy { get; set; }
        [field: SerializeField]
        public int SkillExplosives { get; set; }
        [field: SerializeField]
        public int SkillFirearms { get; set; }
        [field: SerializeField]
        public int SkillIntimidation { get; set; }
        [field: SerializeField]
        public int SkillLockpick { get; set; }
        [field: SerializeField]
        public int SkillMagicEnergyWeapons { get; set; }
        [field: SerializeField]
        public int SkillMechanics { get; set; }
        [field: SerializeField]
        public int SkillMedicine { get; set; }
        [field: SerializeField]
        public int SkillMelee { get; set; }
        [field: SerializeField]
        public int SkillScience { get; set; }
        [field: SerializeField]
        public int SkillSleight { get; set; }
        [field: SerializeField]
        public int SkillSneak { get; set; }
        [field: SerializeField]
        public int SkillSurvival { get; set; }
        [field: SerializeField]
        public int SkillThaumaturgy { get; set; }
        [field: SerializeField]
        public int SkillUnarmed { get; set; }

        private void Awake()
        {
            Strength = BaseActorSpecial.Strength;
            Perception = BaseActorSpecial.Perception;
            Endurance = BaseActorSpecial.Endurance;
            Charisma = BaseActorSpecial.Charisma;
            Intelligence = BaseActorSpecial.Intelligence;
            Agility = BaseActorSpecial.Agility;
            Luck = BaseActorSpecial.Luck;

            UpdateAllSecondaryStats();
            //actorPast.OnEffect(this.gameObject);
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
            SkillThaumaturgy = (2 * BaseActorSpecial.MagicSpecialValue) + (Luck / 2);
            SkillUnarmed = (2 * Endurance) + (Luck / 2);
        }
    }
}
