using System.Collections.Generic;
using Scripts.Items;
using UnityEngine;

namespace Scripts.Actors
{
    /// <summary>
    /// The inventory of an Actor.
    /// </summary>
    [RequireComponent(typeof(ActorStats))]
    public class ActorInventory : MonoBehaviour
    {

        [SerializeField]
        private int numberOfCaps;
        [SerializeField]
        private ItemStats testing;
        [SerializeField]
        private ItemStats testing2;

        /// <summary>
        /// The current amount of weight that this Actor is carrying.
        /// </summary>
        /// <remarks>
        /// This should always be equal to the summation of all item weights currently in 
        /// the Actor's inventory.
        /// </remarks>
        public int CurrentCarryWeight { get; set; }
        /// <summary>
        /// The current number of Quick Slots that this Actor can use.
        /// </summary>
        /// <remarks>
        /// Quick Slots are used to access small sized items for a lower AP cost.
        /// The maximum number that an Actor can use is dependent on the armor that
        /// an Actor is wearing. Usually, civilian clothes have more Quick Slots that
        /// can be used (pockets, shirt pockets, etc).
        /// </remarks>
        public int CurrentQuickSlots { get; set; }
        public int MaxQuickSlots { get; set; }
        /// <summary>
        /// The maximum number of Weapon Slots that this Actor can use.
        /// </summary>
        /// <remarks>
        /// Weapon Slots are used to access items of any size for a lower AP cost.
        /// By default, the maximum number that an Actor can have is 2 slots. However,
        /// this can be modified in the future to account for Perks, PASTs, etc.
        /// </remarks>
        public int MaxWeaponSlots { get; private set; }
        /// <summary>
        /// The Weapon Slots that this Actor is using.
        /// </summary>
        /// <remarks>
        /// This array stores key references to items in the Actor's Inventory. The number
        /// of references in this array should not go over <see cref="MaxWeaponSlots"/>
        /// </remarks>
        public string[] WeaponSlots { get; set; }

        /// <summary>
        /// The item inventory that is associated with this Actor.
        /// </summary>
        /// <remarks>
        /// There are two general types of items that are accounted for in the inventory- stackable
        /// items and non-stackable items. When storing stackable items, the ItemName associated with
        /// the item is used as the Dictionary key. For non-stackable items, a combination of 
        /// the ItemName and the unqiue GUID that is generated for the item instance are
        /// used as the Dictionary key.
        /// </remarks>
        public Dictionary<string, (ItemStats, int)> Inventory { get; private set; }

        private ActorStats actorStats;

        // Slots should store references to items in Inventory
        // Inventory- need to figure out a way to store items and keep track of quantity of said items
        // Use ItemStats for now, if need to be more general, use GameObject
        // (similar to how ActorStats is being used)
        // Need to watch out for duplicates- how to deal with that

        private void Awake()
        {
            MaxWeaponSlots = 2;
            WeaponSlots = new string[MaxWeaponSlots];
            Inventory = new Dictionary<string, (ItemStats, int)>();
        }

        // Start is called before the first frame update
        void Start()
        {
            actorStats = GetComponent<ActorStats>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Creates and returns the proper Dictionary key to store this item with in this Actor's inventory.
        /// </summary>
        /// <param name="itemStats">The general information associated with this item.</param>
        /// <returns>Proper string key to store this item with in the Inventory Dictionary.</returns>
        public string GetProperItemKey(ItemStats itemStats)
        {
            BaseItemStats baseItemInfo = itemStats.BaseItemInfo;
            string itemUniqueId = itemStats.UniqueId;

            string itemKey = baseItemInfo.IsStackable ? baseItemInfo.ItemName : baseItemInfo.ItemName + "-" + itemUniqueId;
            return itemKey;
        }

        /// <summary>
        /// Adds item to one of Actor's Weapon Slots.
        /// </summary>
        /// <remarks>
        /// Note that a string is being stored, which is then used to reference a (ItemStats, int) tuple.
        /// </remarks>
        /// <param name="index">The index of the Weapon Slot that this item should be added to.</param>
        /// <param name="itemStats">The general item information of the item being added.</param>
        public void AddToWeaponSlot(int index, ItemStats itemStats)
        {
            if(index < 0 || index >= MaxWeaponSlots)
            {
                Debug.LogError("An item is trying to be added to an index outside of the Weapon Slots!");
            }


            WeaponSlots[index] = GetProperItemKey(itemStats);
        }

        /// <summary>
        /// Returns item tuple that is stored in Weapon Slot.
        /// </summary>
        /// <param name="index">The index of the Weapon Slot being accessed.</param>
        /// <returns>A (ItemStats, int) tuple of the weapon stored in the Actor's inventory.</returns>
        public (ItemStats, int) GetWeaponFromSlot(int index)
        {
            if (index < 0 || index >= WeaponSlots.Length)
            {
                Debug.LogError("An item is trying to be retrieved from an index outside of the Weapon Slots!");
            }
            string itemKey = WeaponSlots[index];
            
            //Can't pass null as key to TryGetValue
            if(itemKey == null)
            {
                return (null, 0);
            }

            //Using TryGetValue automatically handles keys that are not found
            Inventory.TryGetValue(itemKey, out (ItemStats, int) value);

            return value;
        }

        /// <summary>
        /// Adds item to this Actor's inventory.
        /// </summary>
        /// <param name="itemStats">The general item information of this item being added.</param>
        public void AddItemToInventory(ItemStats itemStats)
        {

            string itemKey = GetProperItemKey(itemStats);
            if(!Inventory.ContainsKey(itemKey) || !itemStats.BaseItemInfo.IsStackable)
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

        /// <summary>
        /// Gets string of items stored in Actor's Weapon Slots.
        /// </summary>
        /// <returns>Combined string of items stored in Actor's Weapon Slots.</returns>
        public string GetWeaponSlotStrings()
        {
            string weaponSlotString = string.Empty;
            for(int i = 0; i < WeaponSlots.Length; i++)
            {
                (ItemStats, int) weaponTuple = GetWeaponFromSlot(i);

                string weaponName = weaponTuple.Item1 != null ? weaponTuple.Item1.BaseItemInfo.ItemName : "null";
                string weaponCount = weaponTuple.Item2.ToString();
                weaponSlotString += $"WeaponSlot[{i}] = ({weaponName}, {weaponCount})\n";
            }

            return weaponSlotString;
        }

        /// <summary>
        /// Gets string of items stored in Actor's inventory.
        /// </summary>
        /// <returns>Combined string of items stored in Actor's inventory.</returns>
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
}
