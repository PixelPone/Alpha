using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareSelection
{
    private Vector2Int originPoint;
    private HashSet<Vector2Int> selectionArea;
    public readonly List<Vector2Int> selectionAreaList;

    public HashSet<Vector2Int> SelectionArea {  get { return selectionArea; } }

    public SquareSelection(Vector2Int originPoint, int width, int height)
    {
        this.originPoint = originPoint;
        selectionArea = new HashSet<Vector2Int>();

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                selectionArea.Add(new Vector2Int(originPoint.x + i, originPoint.y + j));
            }
        }

        selectionAreaList = new List<Vector2Int>(selectionArea);
    }

}
