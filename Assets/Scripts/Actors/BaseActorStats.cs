using UnityEngine;
using static Constants;

namespace Assets.Scripts.Actors
{
    /// <summary>
    /// Stores an Actor's general information and Base SPECIAL values.
    /// </summary>
    [CreateAssetMenu(fileName = "BaseActorStats", menuName = "Alpha/BaseActorStats")]
    public class BaseActorStats : ScriptableObject
    {
        [field: SerializeField, TextArea, Header("General Info")]
        public string Description { get; private set; }
        [field: SerializeField]
        public string RaceName { get; private set; } = "Earth";

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

        public void Awake()
        {
            /*Debug.Log("On BaseActorStat Awake!");
            Debug.Log($"strength {Strength}");
            Debug.Log($"perception {Perception}");
            Debug.Log($"endurance {Endurance}");
            Debug.Log($"charisma {Charisma}");
            Debug.Log($"intelligence {Intelligence}");
            Debug.Log($"agility {Agility}");
            Debug.Log($"luck {Luck}");*/

            UpdateMagicSpecial();
        }

        public void OnValidate()
        {
            PrintSpecialValues();
            UpdateMagicSpecial();
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

            PrintSpecialValues();
            UpdateMagicSpecial();
        }

        private void PrintSpecialValues()
        {
            /*Debug.Log("On BaseActorStat OnValidate!");
            Debug.Log($"strength {strength}");
            Debug.Log($"perception {perception}");
            Debug.Log($"endurance {endurance}");
            Debug.Log($"charisma {charisma}");
            Debug.Log($"intelligence {intelligence}");
            Debug.Log($"agility {agility}");
            Debug.Log($"luck {luck}");*/
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
