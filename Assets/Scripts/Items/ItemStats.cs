using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "Dummy_Item", menuName = "Alpha/Item")]
public class ItemStats : ScriptableObject
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
