using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Actors
{
    [RequireComponent(typeof(ActorSpecial))]
    [RequireComponent(typeof(ActorInfo))]
    public class ActorStatus : MonoBehaviour
    {
        //Used to get Level
        private ActorInfo actorInfo;

        //Used to get SPECIAL values needed for stat calculations
        private ActorSpecial actorSpecial;

        //Used to get base Resistances
        private BaseActorStats baseActorStats;

        //Properties for cleaner access of ActorSpecial Specials
        private int Strength { get { return actorSpecial.Strength; } }
        private int Perception { get { return actorSpecial.Perception; } }
        private int Endurance { get { return actorSpecial.Endurance; } }
        private int Charisma { get { return actorSpecial.Charisma; } }
        private int Intelligence { get { return actorSpecial.Intelligence; } }
        private int Agility { get { return actorSpecial.Agility; } }
        private int Luck { get { return actorSpecial.Luck; } }

        //Status Stats
        //Pain Thresholds at 5 HP, 3 HP, and 1 HP
        
        //Max Thresholds
        public int MaxHp { get; private set; }
        public int MaxAp { get; private set; }
        //Default = 20, +5 every 5 levels
        public int MaxStrain { get; private set; }
        public int MaxInsanity { get; private set; }

        //Current Values
        private int currentHp;
        private int currentAp;
        private int currentStrain;
        private int currentInsanity;

        public int CurrentHp { get { return currentHp; } set { currentHp = Mathf.Clamp(value, 0, MaxHp); } }
        public int CurrentAp { get { return currentAp; } set { currentAp = Mathf.Clamp(value, 0, MaxAp); } }
        public int CurrentStrain { get { return currentStrain; } set { currentStrain = Mathf.Clamp(value, 0, MaxStrain); } }
        public int CurrentInsanity { get { return currentInsanity; } set { currentInsanity = Mathf.Clamp(value, 0, MaxInsanity); } }

        public int HealingRate { get; private set; }

        //Base Resistances
        public int BaseResistancePoision { get { return baseActorStats.ResistancePoision; } }
        public int BaseResistanceRadiation { get { return baseActorStats.ResistanceRadiation; } }
        //Used for SPECIAL Check
        public int BaseResistanceCold { get { return baseActorStats.ResistanceCold; } }
        //Used for Endurance SPECIAL Check
        public int BaseResistanceHeat { get { return baseActorStats.ResistanceHeat; } }
        //Used for SPECIAL Check
        public int BaseResistanceElectricity { get { return baseActorStats.ResistanceElectricity; } }

        //Current Resistances
        public int ResistancePoision { get; private set; }
        public int ResistanceRadiation { get; private set; }
        public int ResistanceCold { get; private set; }
        public int ResistanceHeat { get; private set; }
        public int ResistanceElectricity { get; private set; }

        [SerializeField]
        private List<ActorLimb> actorLimbs;

        private void Awake()
        {
            actorLimbs = new List<ActorLimb>();
        }

        // Use this for initialization
        void Start()
        {
            actorInfo = GetComponent<ActorInfo>();
            actorSpecial = GetComponent<ActorSpecial>();
            baseActorStats = actorSpecial.BaseActorSpecial;
            SetStats();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetStats()
        {
            MaxHp = 10 + Endurance;
            MaxAp = 10 + (Agility / 2);
            int strainBonus = (actorInfo.Level / 5);
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

            //Set Current Resistance stats

            ResistancePoision = 10;
            ResistanceRadiation = 5;
            ResistanceCold = Endurance / Agility;
            ResistanceHeat = Endurance;
            ResistanceElectricity = Endurance / Strength;
        }
    }
}