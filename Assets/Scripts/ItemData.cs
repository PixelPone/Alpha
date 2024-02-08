using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Alpha/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemType;
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private bool canStack;
    [SerializeField] private int stackLimit;

    public string ItemName { get { return itemName; } set { itemName = value; } }
    public string ItemType { get { return itemType; } set { itemType = value; } }
    public string ItemDescription { get { return itemDescription; } set { itemDescription = value; } }
    public Sprite ItemSprite { get { return itemSprite; } set { itemSprite = value; } }
    public Sprite ItemIcon { get { return itemIcon; } set { itemIcon = value; } }
    public bool CanStack { get { return canStack; } set { canStack = value; } }
    public int StackLimit { get { return stackLimit; } set { stackLimit = value; } }
}
