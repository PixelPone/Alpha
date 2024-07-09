using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorStats))]
public class ActorInventory : MonoBehaviour
{

    [SerializeField]
    private int numberOfCaps;
    [SerializeField]
    private ItemStats testing;
    [SerializeField]
    private ItemStats testing2;

    public int CurrentCarryWeight { get; set; }
    public int CurrentQuickSlots { get; set; }
    public int MaxQuickSlots { get; set; }
    public int MaxWeaponSlots { get; private set; }

    public List<string> WeaponSlots { get; set; }

    // Slots should store references to items in Inventory
    // Inventory- need to figure out a way to store items and keep track of quantity of said items
    // Use ItemStats for now, if need to be more general, use GameObject
    // (similar to how ActorStats is being used)

    //Need to watch out for duplicates- how to deal with that

    public Dictionary<string, (ItemStats, int)> Inventory { get; private set; }

    private ActorStats actorStats;

    private void Awake()
    {
        MaxWeaponSlots = 2;
        WeaponSlots = new List<string>(MaxWeaponSlots);
        Inventory = new Dictionary<string, (ItemStats, int)>();
    }

    // Start is called before the first frame update
    void Start()
    {
        actorStats = GetComponent<ActorStats>();
        AddItemToInventory(testing);
        AddItemToInventory(testing2);
        AddToWeaponSlot(0, testing);
        Debug.Log(GetInventoryString());
        Debug.Log(GetWeaponSlotStrings());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetProperItemKey(ItemStats itemStats)
    {
        BaseItemStats baseItemInfo = itemStats.BaseItemInfo;
        string itemUniqueId = itemStats.UniqueId;

        string itemKey = baseItemInfo.IsStackable ? baseItemInfo.ItemName : baseItemInfo.ItemName + "-" + itemUniqueId;
        return itemKey;
    }

    public void AddToWeaponSlot(int index, ItemStats itemStats)
    {
        if(index < 0 || index >= WeaponSlots.Count)
        {
            Debug.LogError("An item is trying to be added to an index outside of the Weapon Slots!");
        }

        WeaponSlots[index] = GetProperItemKey(itemStats);
    }

    public (ItemStats, int) GetWeaponFromSlot(int index)
    {
        if (index < 0 || index >= WeaponSlots.Count)
        {
            Debug.LogError("An item is trying to be retrieved from an index outside of the Weapon Slots!");
        }
        string itemKey = WeaponSlots[index];
        //Using TryGetValue automatically handles keys that are not found
        Inventory.TryGetValue(itemKey, out (ItemStats, int) value);

        return value;
    }

    public void AddItemToInventory(ItemStats itemStats)
    {

        string itemKey = GetProperItemKey(itemStats);
        if(!Inventory.ContainsKey(itemKey) || !itemStats.IsStackable())
        {
            Inventory[itemKey] = (itemStats, 1);
        }
        else
        {
            (ItemStats, int) currentTuple = Inventory[itemKey];
            (ItemStats, int) updatedTuple = (currentTuple.Item1, currentTuple.Item2 + 1);
            Inventory[itemKey] = updatedTuple;
        }

        //Are we doing OverEncumbered as a status?
        int itemWeight = itemStats.BaseItemInfo.Weight;
        CurrentCarryWeight += itemWeight;
    }

    public string GetWeaponSlotStrings()
    {
       string weaponSlotString = string.Empty;
       for(int i = 0; i < WeaponSlots.Count; i++)
       {
            (ItemStats, int) weaponTuple = GetWeaponFromSlot(i);
            string weaponName = weaponTuple.Item1.BaseItemInfo.ItemName;
            string weaponCount = weaponTuple.Item2.ToString();
            weaponSlotString += $"WeaponSlot[{i}] = ({weaponName}, {weaponCount})\n";
        }

       return weaponSlotString;
    }

    public string GetInventoryString()
    {
        string inventoryString = string.Empty;
        if(Inventory.Count == 0)
        {
            inventoryString = "Inventory is Empty!";
            return inventoryString;
        }

        foreach(KeyValuePair<string, (ItemStats, int)> keyValuePair in Inventory)
        {
            string itemName = keyValuePair.Value.Item1.BaseItemInfo.ItemName;
            string itemCount = keyValuePair.Value.Item2.ToString();
            inventoryString += $"Inventory[{keyValuePair.Key}] = ({itemName}, {itemCount})\n";
        }

        return inventoryString;
    }
}
