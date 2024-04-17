using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private ItemData weaponData;
    public int CurrentAmmoCount {  get; private set; }
    [SerializeField] private int maxAmmoCount;
    [SerializeField] private BattleEntityStats user;

    public int MaxAmmoCount { get { return maxAmmoCount; } set { maxAmmoCount = value; } }
    public ItemData WeaponData { get { return weaponData; } set { weaponData = value; } }
    public BattleEntityStats User { get { return user; } private set { user = value; } }
    public List<GameObject> Targets {  get; private set; }

    public void Awake()
    {
        CurrentAmmoCount = maxAmmoCount;
        Targets = new List<GameObject>();
    }
}
