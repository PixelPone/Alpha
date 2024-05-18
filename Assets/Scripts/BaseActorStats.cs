using System.Collections;
using System.Collections.Generic;
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

        UpdateMagicSpecial();
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

        UpdateMagicSpecial();
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

        UpdateMagicSpecial();
    }

    private void UpdateMagicSpecial()
    {
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
                magicSpecialValue = 0;
                break;
            default:
                magicSpecialValue = strength;
                break;
        }
    }
}
