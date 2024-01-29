using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PyramidSelection : SelectionBase
{
    public PyramidSelection(Vector2Int originPoint, int width)
    {
        OriginPoint = originPoint;
        SelectionArea = new HashSet<Vector2Int>();

        int currentOdd = 1;

        for (int i = 0; i < width; i++)
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
}
