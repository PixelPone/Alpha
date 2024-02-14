using System.Collections.Generic;
using UnityEngine;

public abstract class SelectionBase
{
    public Vector2Int OriginPoint { get; set; }
    public HashSet<Vector2Int> SelectionArea { get; set; }
    [SerializeField] private List<Vector2Int> selectionAreaList;
    public List<Vector2Int> SelectionAreaList { get { return selectionAreaList; } set { selectionAreaList = value; } }

    public abstract void UpdateSelectionArea(Vector2Int newOriginPoint);
}
