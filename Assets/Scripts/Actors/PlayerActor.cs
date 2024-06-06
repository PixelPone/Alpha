using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : StateMachineCore
{
    private ActorStats actorStats;

    private void Awake()
    {
        actorStats = GetComponent<ActorStats>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetUpInstances();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
