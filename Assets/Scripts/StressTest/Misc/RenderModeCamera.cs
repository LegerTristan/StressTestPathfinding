using UnityEngine;

/// <summary>
/// Camera class used in render mode
/// </summary>
public class RenderModeCamera : MonoBehaviour
{
    #region F/P
    [SerializeField]
    Camera cam = null;

    [SerializeField, Range(5, 26)]
    float minOrthoSize = 5, maxOrthoSize = 20;

    [SerializeField, Range(1f, 5f)]
    float orthoSizeScalar = 1.75f;

    bool IsPathfinderCamValid => cam != null;
    #endregion

    #region Methods
    public void Init(GridMono _gridTest)
    {
        if (!IsPathfinderCamValid)
            return;

        _gridTest.OnGridGenerated += (_grid, _row, _column) => SetOrthoSize(_row, _column);
    }

    /// <summary>
    /// Set orthographic size based on current grid row and column.
    /// </summary>
    /// <param name="_row">Number of rows on the grid</param>
    /// <param name="_column">Number of columns on the grid</param>
    void SetOrthoSize (int _row, int _column)
    {
        float _rowOrthoSize = _row * orthoSizeScalar;
        _rowOrthoSize = _rowOrthoSize > maxOrthoSize ? maxOrthoSize : _rowOrthoSize;
        _rowOrthoSize = _rowOrthoSize < minOrthoSize ? minOrthoSize : _rowOrthoSize;

        float _columnOrthoSize = _column * orthoSizeScalar;
        _columnOrthoSize = _columnOrthoSize > maxOrthoSize ? maxOrthoSize : _columnOrthoSize;
        _columnOrthoSize = _columnOrthoSize < minOrthoSize ? minOrthoSize : _columnOrthoSize;

        cam.orthographicSize = Mathf.Lerp(_rowOrthoSize, _columnOrthoSize, 0.5f);
    }
    #endregion
}
