using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base UI class for each mode of the program.
/// Contains commons UI widgets.
/// </summary>
public abstract class GraphAlgoUI : MonoBehaviour
{
    #region F/P
    const string TEXT_START_ALGO = "Start Algorithm",
                 TEXT_RESET_ALGO = "Reset Algorithm";

    protected GridMono gridMono = null;

    protected PathfinderMono pathfinder = null;

    protected SearcherMono searcher = null;

    #region Widgets
    [SerializeField, Header("Input Fields")]
    protected ClampedInputField_Int inputRow = null;

    [SerializeField]
    protected ClampedInputField_Int inputColumn = null, inputAlgoExecutionCount = null, inputRange = null;

    [SerializeField, Header("Button")]
    protected Button btnRegenerate = null;

    [SerializeField]
    protected Button btnAlgorithm = null;

    [SerializeField]
    TMP_Text txtAlgorithm = null;

    [SerializeField, Header("Algo Type")]
    protected DropdownAlgoTypes ddAlgoType = null;

    [SerializeField, Header("Pathfinding Algo")]
    protected DropdownPathfindingAlgo ddPathfindingAlgo = null;

    [SerializeField, Header("Searching Algo")]
    protected DropdownSearchingAlgo ddSearchingAlgo = null;

    [SerializeField, Header("ElapsedTime")]
    TMP_Text txtElapsedTime = null, txtAverageTime = null;
    #endregion

    protected AlgoType algoType = AlgoType.SEARCH;

    public AlgoType AlgoType => algoType;

    protected bool isProcessing = false;

    protected bool IsGraphAlgoValid => gridMono && pathfinder && searcher && inputRow && inputColumn
    && btnRegenerate && btnAlgorithm && txtAlgorithm && inputAlgoExecutionCount && inputRange
    && txtElapsedTime && txtAverageTime && ddAlgoType && ddPathfindingAlgo && ddSearchingAlgo;

    #endregion

    #region CustomMethods
    protected virtual void InitGraphAlgoUI(GridMono _gridMono, PathfinderMono _pathfinder, 
        SearcherMono _searcher)
    {
        gridMono = _gridMono;
        pathfinder = _pathfinder;
        searcher = _searcher;

        if (!IsGraphAlgoValid)
            return;

        InitListeners();
        InitFields();
    }

    protected virtual void RegenerateGrid()
    {
        gridMono.Row = inputRow.InputValue;
        gridMono.Column = inputColumn.InputValue;
    }

    void InitListeners()
    {
        pathfinder.OnPathfindingStarted += SwitchAlgorithmButton;
        searcher.OnSearchingStarted += SwitchAlgorithmButton;

        pathfinder.OnPathfindingReset += (_reach, _vis, _reslt, _st, _end) => SwitchAlgorithmButton();
        searcher.OnSearchingReset += (_reach, _vis, _reslt, _st) => SwitchAlgorithmButton();

        btnRegenerate.onClick.AddListener(RegenerateGrid);

        SetAlgoButtonListener();

        ddAlgoType.OnEnumValueChanged += SetAlgoType;
        ddPathfindingAlgo.OnEnumValueChanged += SetInputRangeInteractable;

        pathfinder.Chrono.OnChronoElapsed += SetElapsedTime;
        pathfinder.Chrono.OnChronoAverageElapsed += SetAverageTime;

        searcher.Chrono.OnChronoElapsed += SetElapsedTime;
        searcher.Chrono.OnChronoAverageElapsed += SetAverageTime;
    }

    protected abstract void InitFields();

    /// <summary>
    /// Set algorithm button listeners and text based on isProcessing.
    /// </summary>
    void SetAlgoButtonListener()
    {
        if (isProcessing)
        {
            btnAlgorithm.onClick.RemoveListener(StartAlgorithm);
            btnAlgorithm.onClick.AddListener(ResetAlgorithm);
            txtAlgorithm.text = TEXT_RESET_ALGO;
        }
        else
        {
            btnAlgorithm.onClick.RemoveListener(ResetAlgorithm);
            btnAlgorithm.onClick.AddListener(StartAlgorithm);
            txtAlgorithm.text = TEXT_START_ALGO;
        }
    }

    protected abstract void StartAlgorithm();

    void ResetAlgorithm()
    {
        searcher.ResetAlgorithm();
        pathfinder.ResetAlgorithm();
    }

    /// <summary>
    /// Invert processing state and btnRegenerate interaction, 
    /// then setup listeners and text for algorithm button
    /// </summary>
    protected void SwitchAlgorithmButton()
    {
        btnRegenerate.interactable = !btnRegenerate.interactable;
        isProcessing = !isProcessing;
        SetAlgoButtonListener();
    }

    protected virtual void SetAlgoType(AlgoType _value)
    {
        algoType = _value;

        SetInputRangeInteractable(ddPathfindingAlgo.Value);
    }

    /// <summary>
    /// Set input range interactable in function of algorithm type or pathfinding algorithm type.
    /// </summary>
    /// <param name="_value">Current pathfinding algo type</param>
    void SetInputRangeInteractable(PathfindingAlgo _value)
    {
        if (algoType == AlgoType.SEARCH || _value == PathfindingAlgo.IDDFS)
            inputRange.SetInteractable(true);
        else
            inputRange.SetInteractable(false);
    }

    protected void SetElapsedTime(string _value) => txtElapsedTime.text = _value;

    protected void SetAverageTime(string _value) => txtAverageTime.text = _value;
    #endregion
}
