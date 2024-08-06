using Scripts.CombatStates;
using Scripts.Perks;
using UnityEngine;
using static Scripts.Constants;

namespace Scripts.Actors
{
    public class ActorSpecial : MonoBehaviour
    {
        [field: Header("General Info"), SerializeField]
        public string ActorName { get; private set; }
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        public int Karma { get; set; } = 0;

        [SerializeField]
        private ActorPast actorPast;
        [field: SerializeField]
        public Actor_Gender Gender { get; private set; } = Actor_Gender.MALE;

        public BaseActorStats BaseStats { get; private set; }

        //Any Battle Specific variables of an Actor- if there are too many, this will
        //be refractored into a separate component
        [field: SerializeField]
        public CombatState InitialCombatEvent { get; private set; }

        [field: SerializeField]
        public Vector2Int BattleGridPosition { get; set; }

        //Actor's SPECIAL stats
        //Ranges [1 - 10] normally, can go up to 15 with temporary buffs
        //Base SPECIAL is currently retrived from BaseActorStats SO
        //Permanent SPECIAL is stored and retrived from ActorSpecial
        //This is so that each indiviual Actor can have their own SPECIAL if needed

        //Base SPECIAL
        public int BaseStrength { get { return BaseStats.Strength; } }
        public int BasePerception { get { return BaseStats.Perception; } }
        public int BaseEndurance { get { return BaseStats.Endurance; } }
        public int BaseCharisma { get { return BaseStats.Charisma; } }
        public int BaseIntelligence { get { return BaseStats.Intelligence; } }
        public int BaseAgility { get { return BaseStats.Agility; } }
        public int BaseLuck { get { return BaseStats.Luck; } }

        //Permanent SPECIAL for this Actor
        //This is Base SPECIAL modified with any Perks, Traits, etc
        //(Any permanent modifiers)
        public int Strength { get; set; }
        public int Perception { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Intelligence { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }

        //For Temporary SPECIAL buffs, the maximum value that these can be is 15
        private int tempStrength;
        private int tempPerception;
        private int tempEndurance;
        private int tempCharisma;
        private int tempIntelligence;
        private int tempAgility;
        private int tempLuck;

        public int TempStrength { get { return tempStrength; } set { tempStrength = Mathf.Clamp(value, 1, 15); } }
        public int TempPerception { get { return tempPerception; } set { tempPerception = Mathf.Clamp(value, 1, 15); } }
        public int TempEndurance { get { return tempEndurance; } set { tempEndurance = Mathf.Clamp(value, 1, 15); } }
        public int TempCharisma { get { return tempCharisma; } set { tempCharisma = Mathf.Clamp(value, 1, 15); } }
        public int TempIntelligence { get { return tempIntelligence; } set { tempIntelligence = Mathf.Clamp(value, 1, 15); } }
        public int TempAgility { get { return tempAgility; } set { tempAgility = Mathf.Clamp(value, 1, 15); } }
        public int TempLuck { get { return tempLuck; } set { tempLuck = Mathf.Clamp(value, 1, 15); } }

        public void Awake()
        {
            Strength = BaseStrength;
            Perception = BasePerception;
            Endurance = BaseEndurance;
            Charisma = BaseCharisma;
            Intelligence = BaseIntelligence;
            Agility = BaseAgility;
            Luck = BaseLuck;

            TempStrength = Strength;
            TempPerception = Perception;
            TempEndurance = Endurance;
            TempCharisma = Charisma;
            TempIntelligence = Intelligence;
            TempAgility = Agility;
            TempLuck = Luck;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}