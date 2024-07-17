using Scripts.CombatStates;
using Scripts.Perks;
using System.Collections;
using UnityEngine;

namespace Scripts.Actors
{
    public class ActorSpecial : MonoBehaviour
    {
        //Will get refractored into its own component
        [field: Header("General Info"), SerializeField]
        public BaseActorStats BaseActorSpecial { get; private set; }
        [SerializeField]
        private string actorName;
        public int Level { get; set; } = 1;
        private int experience = 0;
        private int karma;

        [SerializeField]
        private ActorPast actorPast;

        //Any Battle Specific variables of an Actor- if there are too many, this will
        //be refractored into a separate component
        [field: SerializeField]
        public CombatState InitialCombatEvent { get; private set; }

        [field: SerializeField]
        public Vector2Int BattleGridPosition { get; set; }

        //Actor's SPECIAL stats
        //Ranges [1 - 10] normally, can go up to 15 with temporary buffs
        //This is so that each indiviual Actor can have their own SPECIAL if needed
        //(useful for permenant modifiers)
        [field: Header("SPECIAL Values")]
        public int Strength { get; set; }
        public int Perception { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Intelligence { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }

        //For Temporary SPECIAL buffs, the maximum value that these can be is 15
        [Header("Temporary SPECIAL Values")]
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
            Strength = BaseActorSpecial.Strength;
            Perception = BaseActorSpecial.Perception;
            Endurance = BaseActorSpecial.Endurance;
            Charisma = BaseActorSpecial.Charisma;
            Intelligence = BaseActorSpecial.Intelligence;
            Agility = BaseActorSpecial.Agility;
            Luck = BaseActorSpecial.Luck;

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