using System.Collections.Generic;
using UnityEngine;

namespace Scripts.CombatStates.SelectionAreas
{
    public abstract class SelectionAreaBase : MonoBehaviour
    {
        protected Vector2Int originPoint;
        protected HashSet<Vector2Int> selectionArea;
        protected List<Vector2Int> selectionAreaList;

        public void Awake()
        {
            originPoint = new Vector2Int(0, 0);
            selectionArea = new HashSet<Vector2Int>();
            selectionAreaList = new List<Vector2Int>();
        }

        public bool ContainsPosition(Vector2Int position)
        {
            return selectionArea.Contains(position);
        }

        public abstract void UpdateSelectionArea(Vector2Int newOriginPoint);
        public abstract void ResetValues();
    }
}