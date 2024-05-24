using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField]
    private ItemStats itemStats;

    [SerializeField]
    private string damageString;


    private void Awake()
    {
        Dice testDice = new Dice(damageString);
        if(testDice.ListOfDice.Count > 1)
        {
            Debug.LogWarning("The number of dice for this weapon is greater than 1!");
        }
        
        foreach((int, int, int) die in testDice.ListOfDice)
        {
            if(die.Item2 != 6)
            {
                Debug.LogWarning("The number of sides of dice being used is not 6");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
