using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PyramidSelection : SelectionBase
{
    [SerializeField] private int width;

    public PyramidSelection()
    {
        SelectionArea = new HashSet<Vector2Int>();
        SelectionAreaList = new List<Vector2Int>(SelectionArea);
    }

    public PyramidSelection(Vector2Int originPoint, int width)
    {
        OriginPoint = originPoint;
        SelectionArea = new HashSet<Vector2Int>();
        this.width = width;

        int currentOdd = 1;

        for (int i = 0; i < this.width; i++)
        {
            int columnX = originPoint.x + i;
            int columnBottom = originPoint.y - i;

            for(int j = 0; j < currentOdd; j++)
            {
                Vector2Int newIndex = new Vector2Int(columnX, columnBottom + j);
                SelectionArea.Add(newIndex);
            }

            currentOdd += 2;
        }

        SelectionAreaList = new List<Vector2Int>(SelectionArea);

    }

    public override void UpdateSelectionArea(Vector2Int newOriginPoint)
    {
        OriginPoint = newOriginPoint;
        SelectionArea.Clear();
        SelectionAreaList.Clear();

        int currentOdd = 1;

        for (int i = 0; i < this.width; i++)
        {
            int columnX = OriginPoint.x + i;
            int columnBottom = OriginPoint.y - i;

            for (int j = 0; j < currentOdd; j++)
            {
                Vector2Int newIndex = new Vector2Int(columnX, columnBottom + j);
                SelectionArea.Add(newIndex);
            }

            currentOdd += 2;
        }

        SelectionAreaList = new List<Vector2Int>(SelectionArea);
    }
}
