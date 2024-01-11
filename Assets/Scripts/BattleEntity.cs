using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : MonoBehaviour
{
    [SerializeField] private BattleEntityData battleEntityData;
    [SerializeField] private GameObject defaultBehavior;
    public Vector2 BattleGridPosition { get; set; }
}
