using UnityEngine;
using static Scripts.Constants;

namespace Scripts.Items
{
    [CreateAssetMenu(fileName = "BaseItemStats", menuName = "Alpha/BaseItemStats")]
    public class BaseItemStats : ScriptableObject
    {
        [field: SerializeField]
        public string ItemName { get; private set; } = "Dummy Item";
        [field: SerializeField, TextArea(minLines: 5, maxLines: 5)]
        public string SpecialNotes { get; private set; } = "Test Description!";
        [field: SerializeField]
        public Item_Rarity ItemRarity { get; private set; }
        [field: SerializeField]
        public int Weight { get; private set; }
        [field: SerializeField]
        public int Value { get; private set; }
        [field: SerializeField]
        public bool IsStackable { get; private set; }
    }
}