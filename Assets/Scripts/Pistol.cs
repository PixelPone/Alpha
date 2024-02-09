using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private ItemData weaponData;
    public int CurrentAmmoCount {  get; private set; }
    [SerializeField] private int clipSize;
    [SerializeField] private BattleEntity user;

    public int ClipSize { get { return clipSize; } set { clipSize = value; } }
    public ItemData WeaponData { get { return weaponData; } set { weaponData = value; } }
    public BattleEntity User { get { return user; } private set { user = value; } }
    public List<GameObject> Targets {  get; private set; }

    public void Awake()
    {
        CurrentAmmoCount = clipSize;
        Targets = new List<GameObject>();
    }
}
