using System;
using UnityEngine;

public class ItemStats : MonoBehaviour
{

    [field: SerializeField]
    public BaseItemStats BaseItemInfo {  get; private set; }

    public string UniqueId { get; private set; }

    private void Awake()
    {
        UniqueId = Guid.NewGuid().ToString();
    }
}