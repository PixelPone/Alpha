using UnityEngine;
using static Constants;

[CreateAssetMenu(fileName = "DummyItemStats", menuName = "Alpha/ItemStats")]
public class ItemStats : ScriptableObject
{
    [field: SerializeField]
    public string ItemName { get; private set; } = "DummyItem";
    [field: SerializeField]
    public double ItemWeight { get; private set; }
    [field: SerializeField]
    public Item_Rarity Item_Rarity { get; private set; }
    [field: SerializeField]
    public int ItemCost { get; private set; }
    [field: SerializeField]
    public string ItemDescription { get; private set; } = "DummyDescription";
    [field: SerializeField]
    public string ItemNotes { get; private set; } = "DummyNotes";
}
