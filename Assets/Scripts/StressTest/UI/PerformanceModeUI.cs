using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Node = Node<Cell>;

/// <summary>
/// UI class of performance mode part
/// </summary>
public class PerformanceModeUI : GraphAlgoUI
{
    #region F/P
    public event Action OnPerformanceModeHide = null;

    public event Action<PathfindingAlgo, Node, Node, int, int> OnStartPathfindingClicked = null;
    public event Action<SearchingAlgo, Node, int, int> OnStartSearchingClicked = null;

    [SerializeField, Header("Input Fields")]
    ClampedInputField_Int inputStartRow = null;

    [SerializeField]
    ClampedInputField_Int inputStartColumn = null, inputEndRow = null, inputEndColumn = null;

    [SerializeField]
    Button btnRender = null;

    [SerializeField, Header("Nodes")]
    TMP_Text txtNodes = null;

    bool IsPerformanceModeValid => txtNodes && inputStartRow && inputStartColumn && inputEndColumn &&
        inputEndRow && btnRender;
    #endregion

    #region CustomMethods
    public void Init(GridMono _gridMono, PathfinderMono _pathfinder, SearcherMono _searcher)
    {
        InitGraphAlgoUI(_gridMono, _pathfinder, _searcher);
        HidePerformanceMode();
    }

    protected override void InitFields()
    {
        if (!IsPerformanceModeValid)
            return;

        inputRow.SetMaxValue(int.MaxValue);
        inputColumn.SetMaxValue(int.MaxValue);

        inputStartRow.SetMinValue(0);
        inputEndRow.SetMinValue(0);

        inputStartColumn.SetMinValue(0);
        inputEndColumn.SetMinValue(0);

        btnRender.onClick.AddListener(HidePerformanceMode);
    }

    /// <summary>
    /// Call grid MonoBehaviour to create a new grid and set maxValue of row and column inputs field to grid
    /// row and column.
    /// </summary>
    protected override void RegenerateGrid()
    {
        base.RegenerateGrid();
        gridMono.Generate(false);
        txtNodes.text = "Node : " + gridMono.Grid.MaxSize.ToString();

        inputStartRow.SetMaxValue(gridMono.Row - 1);
        inputEndRow.SetMaxValue(gridMono.Row - 1);

        inputStartColumn.SetMaxValue(gridMono.Column - 1);
        inputEndColumn.SetMaxValue(gridMono.Column - 1);
    }

    /// <summary>
    /// Get start node and end node from grid with their input fields value.
    /// Then, invoke StartPathfindingClicked or StartSearchingClicked depending on algo type.
    /// </summary>
    protected override void StartAlgorithm()
    {
        Node _startNode = gridMono.Grid[inputStartRow.InputValue, inputStartColumn.InputValue],
             _endNode = gridMono.Grid[inputEndRow.InputValue, inputEndColumn.InputValue];

        if (algoType == AlgoType.DESTINATION)
            OnStartPathfindingClicked?.Invoke(ddPathfindingAlgo.Value, _startNode, _endNode, 
                inputAlgoExecutionCount.InputValue, inputRange.InputValue);
        else if (algoType == AlgoType.SEARCH)
            OnStartSearchingClicked?.Invoke(ddSearchingAlgo.Value, _startNode, 
                inputAlgoExecutionCount.InputValue, inputRange.InputValue);
    }

    protected override void SetAlgoType(AlgoType _value)
    {
        base.SetAlgoType(_value);

        if (algoType == AlgoType.SEARCH)
        {
            ddSearchingAlgo.SetInteractable(true);
            ddPathfindingAlgo.SetInteractable(false);
            inputEndRow.SetInteractable(false);
            inputEndColumn.SetInteractable(false);
        }
        else
        {
            ddSearchingAlgo.SetInteractable(false);
            ddPathfindingAlgo.SetInteractable(true);
            inputEndRow.SetInteractable(true);
            inputEndColumn.SetInteractable(true);
        }
    }

    /// <summary>
    /// Display performance mode widget and update algorithm type in algo types dropdown.
    /// Then display current number of nodes in textNodes.
    /// </summary>
    /// <param name="_currentType"></param>
    public void ResumePerformanceMode(AlgoType _currentType)
    {
        gameObject.SetActive(true);
        SetAlgoType(_currentType);
        ddAlgoType.SetValue((int)_currentType);

        inputStartRow.SetMaxValue(gridMono.Row - 1);
        inputEndRow.SetMaxValue(gridMono.Row - 1);

        inputStartColumn.SetMaxValue(gridMono.Column - 1);
        inputEndColumn.SetMaxValue(gridMono.Column - 1);

        if (gridMono.Grid != null)
            txtNodes.text = "Node : " + gridMono.Grid.MaxSize.ToString();

    }

    void HidePerformanceMode()
    {
        if (isProcessing)
            return;

        gameObject.SetActive(false);
        OnPerformanceModeHide?.Invoke();
    }
    #endregion
}
