using System;
using UnityEngine;

/// <summary>
/// MonoBehaviour class that contains the grid.
/// </summary>
public class GridMono : MonoBehaviour
{
    #region F/P
    public event Action<Node<Cell>[,], int, int> OnGridGenerated = null;

    [SerializeField]
    Cell prefab = null;

    Grid<Cell> grid = null;

    [SerializeField, Range(1, 100)]
    int row = 1, column = 1, cellSize = 1;

    public Grid<Cell> Grid => grid;

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
    #endregion

    #region CustomMethods
    /// <summary>
    /// Generate a new grid (meaning that we replaced the previous one).
    /// Invoke an event when the grid has been generated.
    /// </summary>
    /// <param name="_generateCells"></param>
    public void Generate(bool _generateCells = true)
    {
        if (grid != null)
            grid.Clear();

        grid = new Grid<Cell>(row, column, _generateCells  ? () =>
        {
            if (prefab == null)
                return null;

            return Instantiate(prefab);
        } : null,
        GetSpawnOrigin(),  cellSize);

        OnGridGenerated?.Invoke(grid.GridArray, row, column);
    }

    public void Init(RenderModeCursor _cursor)
    {
        if (!_cursor)
            return;

        _cursor.OnCursorUpdate += (_pos) =>
        {
            if (grid == null)
                return;

            _cursor.HoveredNode = grid.GetNodeFromWorldPosition(_pos);
        };
    }

    Vector3 GetSpawnOrigin()
    {
        float _x = -(row * cellSize) / 2,
              _z = -(column * cellSize) / 2;

        return new Vector3(_x, 0f, _z);
    }
    #endregion
}
