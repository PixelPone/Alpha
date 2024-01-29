using System.Collections.Generic;
using UnityEngine;

public abstract class SelectionBase
{

    public Vector2Int OriginPoint { get; set; }
    public HashSet<Vector2Int> SelectionArea { get; set; }
    public List<Vector2Int> selectionAreaList;
    public List<Vector2Int> SelectionAreaList { get { return selectionAreaList; } set { selectionAreaList = value; } }
}
