using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RightTriangleSelection : SelectionBase
{
    public RightTriangleSelection(Vector2Int originPoint, int width) : base()
    {
        OriginPoint = originPoint;
        SelectionArea = new HashSet<Vector2Int>();

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j <= i; j++)
            {
                SelectionArea.Add(new Vector2Int(originPoint.x + i, originPoint.y + j));
            }
        }

        SelectionAreaList = new List<Vector2Int>(SelectionArea);
    }
}
