using System.Collections;
using System.Linq;
using UnityEngine;

namespace Scripts.CombatStates.SelectionAreas
{
    public class CenterSquareSelection : SelectionAreaBase
    {

        [SerializeField]
        private int halfWidth;

        // Use this for initialization
        void Start()
        {
            if(halfWidth == 0)
            {
                Debug.LogError("The half width of this Selection can not be 0!");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        public override void UpdateSelectionArea(Vector2Int newOriginPoint)
        {
            originPoint = newOriginPoint;
            ResetValues();

            Vector2Int leftCorner = new Vector2Int(newOriginPoint.x - halfWidth, newOriginPoint.y - halfWidth);

            int width = (halfWidth * 2) + 1;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    selectionArea.Add(new Vector2Int(leftCorner.x + i, leftCorner.y + j));
                }
            }

            selectionAreaList = selectionArea.ToList();
        }

        public override void ResetValues()
        {
            selectionArea.Clear();
            selectionAreaList.Clear();
        }
    }
}