using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareSelection : SelectionBase
{
    public SquareSelection(Vector2Int originPoint, int width, int height) : base()
    {
        OriginPoint = originPoint;
        SelectionArea = new HashSet<Vector2Int>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                SelectionArea.Add(new Vector2Int(originPoint.x + i, originPoint.y + j));
            }
        }

        SelectionAreaList = new List<Vector2Int>(SelectionArea);
    }

}
