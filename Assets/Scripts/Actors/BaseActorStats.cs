using System.Collections.Generic;
using UnityEngine;
using static Scripts.Constants;

namespace Scripts.Actors
{
    /// <summary>
    /// Stores an Actor's shared general information and Base SPECIAL values.
    /// </summary>
    [CreateAssetMenu(fileName = "BaseActorStats", menuName = "Alpha/BaseActorStats")]
    public class BaseActorStats : ScriptableObject
    {
        [field: SerializeField, TextArea, Header("General Info")]
        public string Description { get; private set; } = "This is a test description!";
        [field: SerializeField, Range(-4, 4)]
        public int Size { get; private set; } = 0;

        [field: SerializeField]
        public Race_Name RaceName { get; private set; } = Race_Name.EARTH;

        //SPECIAL stats
        //Ranges [1 - 10] normally, can go up to 15 with temporary buffs
        [field: SerializeField, Range(1, 10), Header("SPECIAL Values")]
        public int Strength { get; private set; } = 5;
        [field: SerializeField, Range(1, 10)]
        public int Perception { get; private set; } = 5;
        [field: SerializeField, Range(1, 10)]
        public int Endurance { get; private set; } = 5;
        [field: SerializeField, Range(1, 10)]
        public int Charisma { get; private set; } = 5;
        [field: SerializeField, Range(1, 10)]
        public int Intelligence { get; private set; } = 5;
        [field: SerializeField, Range(1, 10)]
        public int Agility { get; private set; } = 5;
        [field: SerializeField, Range(1, 10)]
        public int Luck { get; private set; } = 5;

        [field: SerializeField]
        public Special_Name MagicSpecial { get; private set; } = Special_Name.STRENGTH;
        public int MagicSpecialValue { get; private set; }

        [field: Header("Base Resistances")]
        public int ResistancePoision { get; private set; }
        public int ResistanceRadiation { get; private set; }
        //Used for SPECIAL Check
        public int ResistanceCold { get; private set; }
        //Used for Endurance SPECIAL Check
        public int ResistanceHeat { get; private set; }
        //Used for SPECIAL Check
        public int ResistanceElectricity { get; private set; }

        [field: SerializeField]
        public List<ActorLimb> actorLimbs { get; private set; }

        public void Awake()
        {
            //Debug.Log("On BaseActorStat Awake!");
            UpdateMagicSpecial();
            CalculateBaseResistances();
            CheckActorLimbs();
        }

        public void OnValidate()
        {
            //Debug.Log("On BaseActorStat OnValidate!");
            //PrintSpecialValues();
            UpdateMagicSpecial();
            CalculateBaseResistances();
            CheckActorLimbs();
        }

        public void Reset()
        {
            Strength = 5;
            Perception = 5;
            Endurance = 5;
            Charisma = 5;
            Intelligence = 5;
            Agility = 5;
            Luck = 5;

            //Debug.Log("On BaseActorStat Reset!");
            //PrintSpecialValues();
            UpdateMagicSpecial();
            CalculateBaseResistances();
            CheckActorLimbs();
        }

        private void PrintSpecialValues()
        {
            Debug.Log($"Strength {Strength}");
            Debug.Log($"Perception {Perception}");
            Debug.Log($"Endurance {Endurance}");
            Debug.Log($"Charisma {Charisma}");
            Debug.Log($"Intelligence {Intelligence}");
            Debug.Log($"Agility {Agility}");
            Debug.Log($"Luck {Luck}");
        }

        private void CalculateBaseResistances()
        {
            ResistancePoision = 10;
            ResistanceRadiation = 5;
            ResistanceCold = Endurance / Agility;
            ResistanceHeat = Endurance;
            ResistanceElectricity = Endurance / Strength;
        }

        private void CheckActorLimbs()
        {
            if(actorLimbs.Count == 0)
            {
                Debug.LogError("This Actor does not have limbs!");
            }
        }

        private void UpdateMagicSpecial()
        {
            switch (MagicSpecial)
            {
                case Special_Name.STRENGTH:
                    MagicSpecialValue = Strength;
                    break;
                case Special_Name.PERCEPTION:
                    MagicSpecialValue = Perception;
                    break;
                case Special_Name.ENDURANCE:
                    MagicSpecialValue = Endurance;
                    break;
                case Special_Name.CHARISMA:
                    MagicSpecialValue = Charisma;
                    break;
                case Special_Name.INTELLIGENCE:
                    MagicSpecialValue = Intelligence;
                    break;
                case Special_Name.AGILITY:
                    MagicSpecialValue = Agility;
                    break;
                case Special_Name.LUCK:
                    Debug.LogWarning("Magic Special is set to LUCK. This is not a valid SPECIAL!");
                    MagicSpecialValue = 0;
                    break;
                default:
                    MagicSpecialValue = Strength;
                    break;
            }
        }
    }
}
