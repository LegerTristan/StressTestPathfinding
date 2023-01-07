using System;
using UnityEngine;

/// <summary>
/// Create a grid based on a number of row, column and a size and an origin.
/// </summary>
/// <typeparam name="TItem">Item contained in the node of the grid.</typeparam>
public class Grid<TItem> where TItem : MonoBehaviour
{
    #region F/P
    public event Action<Node<TItem>[,]> OnGridReset = null;

    /// <summary>
    /// Multiline array representing the grid.
    /// </summary>
    Node<TItem>[,] gridArray = null;

    /// <summary>
    ///  Origin of the grid
    /// </summary>
    Vector3 origin = Vector3.zero;

    int row = 0, column = 0, nodeSize = 1;

    public Node<TItem>[,] GridArray => gridArray;

    public Node<TItem> this[int _indexX, int _indexY]
    {
        get
        {
            if (_indexX < 0 || _indexY < 0 || _indexX >= row || _indexY >= column)
                return null;

            return gridArray[_indexX, _indexY];
        }
    }

    public int Row
    {
        get => row;
        set => row = value;
    }

    public int Column
    {
        get => column;
        set => column = value;
    }

    public int MaxSize => row * column;

    #endregion

    #region Constructor
    /// <summary>
    /// Create the grid.
    /// </summary>
    /// <param name="_row">Number of row on the grid</param>
    /// <param name="_column">Number of column on the grid</param>
    /// <param name="_createFunc">Function to create item contained in a node.</param>
    /// <param name="_spawnOrigin">Origin of the grid</param>
    /// <param name="_nodeSize">Size of each node.</param>
    public Grid(int _row, int _column, Func<TItem> _createFunc, Vector3 _spawnOrigin = new Vector3(), 
        int _nodeSize = 1)
    {
        if (_row < 1 || _column < 1 || _nodeSize < 1)
            return;

        row = _row;
        column = _column;
        origin = _spawnOrigin;
        nodeSize = _nodeSize;

        gridArray = new Node<TItem>[row, column];

        for(int i = 0; i < row; ++i)
        {
            for (int j = 0; j < column; ++j)
            {
                gridArray[i, j] = new Node<TItem>(_createFunc?.Invoke(), GetWorldPositionFromGrid(i, j), i, j);
            }
        }
    }
    #endregion

    #region CustomMethods
    public Vector3 GetWorldPositionFromGrid(int _x, int _z) => origin + (new Vector3(_x, 0f, _z) * nodeSize);

    /// <summary>
    /// Get a node in the grid from a position in world space.
    /// </summary>
    /// <param name="_worldPos">Position in world space.</param>
    /// <returns>Node to this position, null if this position is outside the grid.</returns>
    public Node<TItem> GetNodeFromWorldPosition(Vector3 _worldPos)
    {
        Vector3 _worldGridPos = (_worldPos - origin) / nodeSize;
        Vector2Int _gridPos = new Vector2Int(Mathf.RoundToInt(_worldGridPos.x), Mathf.RoundToInt(_worldGridPos.z));

        if (_gridPos.x < 0 || _gridPos.y < 0 || _gridPos.x >= row || _gridPos.y >= column)
            return null;

        return gridArray[_gridPos.x, _gridPos.y];
    }

    /// <summary>
    /// Clear the grid and destroy item contained in each node.
    /// </summary>
    public void Clear()
    {
        if (gridArray.Length < row * column)
            return;

        for (int i = 0; i < row; ++i)
        {
            for (int j = 0; j < column; ++j)
            {
                if (gridArray[i, j] != null)
                {
                    gridArray[i, j].Clear();
                    gridArray[i, j] = null;
                }
            }
        }
    }

    /// <summary>
    /// For each node, reset pathfinding data.
    /// </summary>
    //public void ResetNodesPathfinding()
    //{
    //    //if (gridArray.Length < row * column)
    //    //    return;

    //    //for (int i = 0; i < row; ++i)
    //    //    for (int j = 0; j < column; ++j)
    //    //        if (gridArray[i, j] != null)
    //    //            gridArray[i, j].ResetPathfindingMembers();
    //}
    #endregion
}
