using Scripts.CombatStates.SelectionAreas;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.CombatStates
{
    [RequireComponent(typeof(SelectionAreaBase))]
    public class PlayerMoveSelection : CombatState
    {
        /// <summary>
        /// The AP cost of the current potential path being built.
        /// </summary>
        private int costOfCurrentPath;

        /// <summary>
        /// How much AP one movement (1 title) costs.
        /// </summary>
        [SerializeField]
        private int costOfMovement;

        /// <summary>
        /// The start of the current potential path being built.
        /// </summary>
        private Vector2Int startOfCurrentPath;

        /// <summary>
        /// Grid index of center of area that player can select from.
        /// </summary>
        private Vector2Int centerPosition;

        /// <summary>
        /// Current grid index of grid that player is currently hovering over.
        /// </summary>
        private Vector2Int hoverPosition;

        /// <summary>
        /// Bounds in which the player can select each tile of the path they
        /// want to move.
        /// </summary>
        private SelectionAreaBase selectionBounds;

        /// <summary>
        /// List that stores the path that the player makes- used for undo as well.
        /// </summary>
        /// <remarks>
        /// A List is instead of a Stack in order to iterate through the path that
        /// is created.
        /// </remarks>
        private List<Vector2Int> selectMovements;

        private BattleGrid battleGrid;

        public void Awake()
        {
            selectionBounds = GetComponent<SelectionAreaBase>();
        }

        public override void StartState(BattleManager battleManager)
        {
            base.StartState(battleManager);

            costOfCurrentPath = 0;
            selectMovements = new List<Vector2Int>();

            this.battleGrid = battleManager.BattleGridProperty;
            startOfCurrentPath = this.Owner.BattleGridPosition;
            centerPosition = startOfCurrentPath;
            hoverPosition = startOfCurrentPath;
            selectionBounds.UpdateSelectionArea(centerPosition);

            Debug.Log("MoveSelection's StartState method Ran!");
            PlayerInput.Instance.OnMoveAction += PlayerInput_OnMoveAction;
            PlayerInput.Instance.OnSelectAction += PlayerInput_OnSelectAction;
            PlayerInput.Instance.OnAltSelectAction += PlayerInput_OnAltSelectAction;
        }

        public override void UpdateState()
        {
            //throw new System.NotImplementedException();
        }

        public override void EndState()
        {
            Debug.Log("MoveSelection's EndState Ran!");
            costOfCurrentPath = 0;
            startOfCurrentPath = Vector2Int.zero;
            centerPosition = startOfCurrentPath;
            hoverPosition = startOfCurrentPath;
            selectMovements = new List<Vector2Int>();

            PlayerInput.Instance.OnMoveAction -= PlayerInput_OnMoveAction;
            PlayerInput.Instance.OnSelectAction -= PlayerInput_OnSelectAction;
            PlayerInput.Instance.OnAltSelectAction -= PlayerInput_OnAltSelectAction;
        }

        public override bool IsFinished()
        {
            throw new System.NotImplementedException();
        }

        private int GetCostOfPathMovement(Vector2Int movement)
        {
            return movement.x != 0 && movement.y != 0 ? costOfMovement * 2 : costOfMovement;
        }

        private void PlayerInput_OnMoveAction(object sender, PlayerInput.InputActionArgs args)
        {
            Vector2 playerInput = args.callbackContext.ReadValue<Vector2>();
            if (playerInput == Vector2Int.left || playerInput == Vector2Int.right
                || playerInput == Vector2Int.up || playerInput == Vector2Int.down)
            {
                Vector2Int potentialNewPosition = hoverPosition + Vector2Int.RoundToInt(playerInput);

                //Debug.Log($"{this.Owner.name} Input: {playerInput}");
                //Debug.Log($"{this.Owner.name} Input: {Vector2Int.RoundToInt(playerInput)}");
                //Debug.Log($"Potential New Hover Position: {potentialNewPosition}");

                //Is potential position both within bounds and within the grid?
                //(Could possibly refractor this into a separate independent Helper function)
                if (battleGrid.IsGridPositionInBounds(potentialNewPosition)
                    && selectionBounds.ContainsPosition(potentialNewPosition))
                {
                    hoverPosition = potentialNewPosition;
                }

                Debug.Log($"Updated Hover Position: {hoverPosition}");
            }
        }

        private void PlayerInput_OnSelectAction(object sender, PlayerInput.InputActionArgs args)
        {
            Debug.Log("SelectionAction Ran in PlayerMoveSelection!");
            Debug.Log($"The value of the square you are touching is: {battleGrid.GetSquareValue(hoverPosition.x, hoverPosition.y)}");
            Vector2Int movement = hoverPosition - centerPosition;
            int costOfMovement = GetCostOfPathMovement(movement);

            //Player is trying to add another battle tile to the path they are building and is able to
            if (!centerPosition.Equals(hoverPosition) && (costOfCurrentPath + costOfMovement)
                <= this.Owner.CurrentAp)
            {
                Debug.Log($"Position Difference: {movement}");
                selectMovements.Add(movement);
                //Add cost of movement that was just added to path
                costOfCurrentPath += costOfMovement;
                Debug.Log("Position Movement Added!");
                centerPosition = hoverPosition;
                selectionBounds.UpdateSelectionArea(centerPosition);

                //Update green squares showing path so far to account for this new movement
                Vector2Int currentPath = startOfCurrentPath;
                foreach (Vector2Int position in selectMovements)
                {
                    currentPath += position;
                    //battleGrid.DrawSquare(Color.green, currentPath.x, currentPath.y);
                }
            }
            //Player selects the center position, ending the path building process
            else if (centerPosition.Equals(hoverPosition) && selectMovements.Count != 0)
            {
                this.Owner.CurrentAp -= costOfCurrentPath;
                battleManager.NextSubstate();
                //StartCoroutine(MoveAlongPathCoroutine());
            }
            //At this point, player is either trying to extend past what they can move to or
            //hasn't created a path at all
            else
            {
                string feedback = string.Empty;
                if (this.Owner.CurrentAp == 0)
                {
                    feedback = "You do not have enough Action Points to create a path!";
                }
                else if (this.Owner.CurrentAp > 0 && selectMovements.Count == 0)
                {
                    feedback = "You haven't created a Path to move yet!";
                }
                else if (this.Owner.CurrentAp > 0 && selectMovements.Count > 0)
                {
                    feedback = "You do not have enough Action Points to move this far!";
                }

                Debug.Log(feedback);
                //StartCoroutine(DialogPathErrorCoroutine(feedback));
            }
        }

        private void PlayerInput_OnAltSelectAction(object sender, PlayerInput.InputActionArgs args)
        {
            //Debug.Log("AltSelectionAction Ran in MoveSelection!");
            if (selectMovements.Count > 0)
            {
                Vector2Int top = selectMovements[selectMovements.Count - 1];
                selectMovements.RemoveAt(selectMovements.Count - 1);
                Vector2Int reverse = new Vector2Int(-top.x, -top.y);
                int costOfReverse = GetCostOfPathMovement(reverse);
                costOfCurrentPath -= costOfReverse;
                centerPosition += reverse;
                selectionBounds.UpdateSelectionArea(centerPosition);

                //Given the grid was just refreshed, redraw current path up to the this new final step
                Vector2Int currentPath = startOfCurrentPath;
                foreach (Vector2Int position in selectMovements)
                {
                    currentPath += position;
                    //battleGrid.DrawSquare(Color.green, currentPath.x, currentPath.y);
                }
                hoverPosition = centerPosition;

            }
            else
            {
                battleManager.PreviouSubstate();
            }

        }
    }
}
