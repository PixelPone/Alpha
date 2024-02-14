using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CenterSquareSelection : SelectionBase
{

    [SerializeField] private int halfWidth;

    public CenterSquareSelection()
    {
        SelectionArea = new HashSet<Vector2Int>();
        SelectionAreaList = new List<Vector2Int>(SelectionArea);
    }

    public CenterSquareSelection(Vector2Int originPoint, int halfWidth) : base()
    {
        if (halfWidth == 0)
        {
            Debug.LogError("Half width can not be 0!");
        }

        OriginPoint = originPoint;
        SelectionArea = new HashSet<Vector2Int>();
        Vector2Int leftCorner = new Vector2Int(originPoint.x - halfWidth, originPoint.y - halfWidth);
        
        this.halfWidth = halfWidth;
        int width = (halfWidth * 2) + 1;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                SelectionArea.Add(new Vector2Int(leftCorner.x + i, leftCorner.y + j));
            }
        }

        SelectionAreaList = new List<Vector2Int>(SelectionArea);
    }

    public override void UpdateSelectionArea(Vector2Int newOriginPoint)
    {
        OriginPoint = newOriginPoint;
        SelectionArea.Clear();
        SelectionAreaList.Clear();

        Vector2Int leftCorner = new Vector2Int(newOriginPoint.x - halfWidth, newOriginPoint.y - halfWidth);

        int width = (halfWidth * 2) + 1;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                SelectionArea.Add(new Vector2Int(leftCorner.x + i, leftCorner.y + j));
            }
        }

        SelectionAreaList = new List<Vector2Int>(SelectionArea);
    }
}
