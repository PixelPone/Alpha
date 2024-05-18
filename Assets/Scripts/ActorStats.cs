using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStats : MonoBehaviour
{

    private int level = 1;
    private int experience = 0;
    private int karma;
    private int size;

    private int currentHp;
    private int currentAp;
    private int currentStrain;
    private int currentInsanity;


    [SerializeField]
    private BaseActorStats baseActorStats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
