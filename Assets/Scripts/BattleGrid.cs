using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Main grind that BattleEntities occupy during battle.
    /// </summary>
    public class BattleGrid
    {

        private int[,] gridArray;
        /// <summary>
        /// Construtor for creating a new battle grid.
        /// </summary>
        /// <param name="originPoint">The bottom left point of the grid.</param>
        /// <param name="width">The number of cells each row of the grid should have.</param>
        /// <param name="height">The number of cells each column of the grid should have.</param>
        /// <param name="cellSizeX">The width of each cell in pixels.</param>
        /// <param name="cellSizeY">The height of each cell in pixels.</param>
        public BattleGrid(Vector2 originPoint, int width, int height, float cellSizeX, float cellSizeY)
        {
            //GameObject _battleGridObject = new GameObject("BattleGrid");
            OriginPoint = originPoint;
            Width = width;
            Height = height;
            CellSizeX = cellSizeX;
            CellSizeY = cellSizeY;
            gridArray = new int[Width, Height];
            //_battleGridObject.transform.position = OriginPoint;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //GameObject gridTile = new GameObject($"GridTile ({x}, {y})");
                    //gridTile.transform.parent = _battleGridObject.transform;
                    gridArray[x, y] = Random.Range(0, 10);
                    /*Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y + 1), GetWorldPosition(x + 1, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x + 1, y), GetWorldPosition(x + 1, y + 1), Color.white, 100f);*/
                }
            }
        }

        public int Width { get; }
        public int Height { get; }
        public float CellSizeX { get; }
        public float CellSizeY { get; }
        public Vector2 OriginPoint { get; }

        public int GetSquareValue(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                Debug.LogWarning("You are trying to access a grid space outside of the grid array!");
                return -1;
            }
            return gridArray[x, y];
        }

        public void DrawSquare(Color color, int x, int y)
        {
            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), color, 100f);
            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), color, 100f);
            Debug.DrawLine(GetWorldPosition(x, y + 1), GetWorldPosition(x + 1, y + 1), color, 100f);
            Debug.DrawLine(GetWorldPosition(x + 1, y), GetWorldPosition(x + 1, y + 1), color, 100f);
        }

        /// <summary>
        /// Gets the world position of tile in the battle grid.
        /// </summary>
        /// <param name="x">x index of the tile (starting from 0)</param>
        /// <param name="y">y index of the tile (starting from 0)</param>
        /// <returns>World position of the bottom left corner of the tile at x,y on the grid.</returns>
        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2((x * CellSizeX) + OriginPoint.x, (y * CellSizeY) + OriginPoint.y);
        }

        /// <summary>
        /// Returns if the grid position (grid index) is within the bounds of the grid.
        /// </summary>
        /// <remarks>
        /// This is referring to an index in the grid, NOT world position.
        /// </remarks>
        /// <param name="gridPosition">The grid position that is being checked.</param>
        /// <returns>
        /// True- the grid position is within the bounds of the grid.
        /// False- the grid position is not within the bounds of the grid.
        /// </returns>
        public bool IsGridPositionInBounds(Vector2 gridPosition)
        {
            int xPosition = (int)gridPosition.x;
            int yPosition = (int)gridPosition.y;
            //Remember that indexes are from [0, width-1] and [0, height-1]
            return xPosition >= 0 && xPosition < Width && yPosition >= 0 && yPosition < Height;
        }

    }
}