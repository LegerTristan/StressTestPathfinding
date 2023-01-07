using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Ui class of render mode part.
/// </summary>
public class RenderModeUI : GraphAlgoUI
{
    #region F/P
    event Action OnUserSetTimerHandler = null;

    public event Action OnRenderModeHide = null;

    public event Action<PathfindingAlgo, int, int> OnStartPathfindingClicked = null;
    public event Action<SearchingAlgo, int, int> OnStartSearchingClicked = null;

    RenderModeCursor cursor = null;

    [SerializeField]
    ClampedInputField_Float inputWaitTime = null;

    [SerializeField]
    Button btnPerformance = null;

    [SerializeField, Header("Timer")]
    Toggle toggleUseTimer = null;

    [SerializeField, Header("Step By Step")]
    Toggle toggleUseStepByStep = null;

    bool IsRenderModeValid => cursor && toggleUseTimer && inputWaitTime && toggleUseStepByStep;
    #endregion

    #region CustomMethods
    public void Init(GridMono _gridMono, PathfinderMono _pathfinder, SearcherMono _searcher, 
        RenderModeCursor _cursor)
    {
        cursor = _cursor;
        InitGraphAlgoUI(_gridMono, _pathfinder, _searcher);
    }

    protected override void RegenerateGrid()
    {
        cursor.ResetNodes();
        base.RegenerateGrid();
        gridMono.Generate();
    }

    protected override void InitFields()
    {
        if (!IsRenderModeValid)
            return;

        ddPathfindingAlgo.SetDropdownVisible(false);

        toggleUseTimer.onValueChanged.AddListener(SetUseTimer);
        toggleUseTimer.isOn = false;

        toggleUseStepByStep.onValueChanged.AddListener(SetUseStepByStep);
        toggleUseStepByStep.isOn = false;

        OnUserSetTimerHandler += RestrainAlgoExecutionCount;

        btnPerformance.onClick.AddListener(HideRenderMode);
    }

    /// <summary>
    /// Invoke StartPathfindingClicked or StartSearchingClicked depending on algo type.
    /// Also, set wait time during each pathfinding / searching algorithm update.
    /// </summary>
    protected override void StartAlgorithm()
    {
        if (algoType == AlgoType.DESTINATION)
        {
            pathfinder.WaitTime = inputWaitTime.InputValue;
            OnStartPathfindingClicked?.Invoke(ddPathfindingAlgo.Value, inputAlgoExecutionCount.InputValue, 
                inputRange.InputValue);
        }
        else if (algoType == AlgoType.SEARCH)
        {
            searcher.WaitTime = inputWaitTime.InputValue;
            OnStartSearchingClicked?.Invoke(ddSearchingAlgo.Value, inputAlgoExecutionCount.InputValue,
                inputRange.InputValue);
        }
    }

    protected override void SetAlgoType(AlgoType _value)
    {
        base.SetAlgoType(_value);

        if (algoType == AlgoType.SEARCH)
        {
            ddSearchingAlgo.SetDropdownVisible(true);
            ddPathfindingAlgo.SetDropdownVisible(false);
        }
        else
        {
            ddSearchingAlgo.SetDropdownVisible(false);
            ddPathfindingAlgo.SetDropdownVisible(true);
        }
    }

    void SetUseTimer(bool _useTimer)
    {
        pathfinder.UseTimer = _useTimer;
        searcher.UseTimer = _useTimer;
        OnUserSetTimerHandler?.Invoke();
    }

    void SetUseStepByStep(bool _useStepByStep)
    {
        pathfinder.UseStepByStep = _useStepByStep;
        searcher.UseStepByStep = _useStepByStep;
        OnUserSetTimerHandler?.Invoke();
    }

    void RestrainAlgoExecutionCount()
    {
        inputAlgoExecutionCount.SetInteractable(toggleUseTimer.isOn || toggleUseStepByStep.isOn ? false : true);
        if (toggleUseTimer.isOn || toggleUseStepByStep.isOn)
            inputAlgoExecutionCount.SetToMinValue();
    }

    /// <summary>
    /// Display RenderMode widget and set current algo type selected in algotype dropdown.
    /// </summary>
    /// <param name="_currentType">Current algo type selected</param>
    public void ResumeRenderMode(AlgoType _currentType)
    {
        gameObject.SetActive(true);
        SetAlgoType(_currentType);
        ddAlgoType.SetValue((int)_currentType);
    }

    void HideRenderMode()
    {
        if (isProcessing)
            return;

        gameObject.SetActive(false);
        OnRenderModeHide?.Invoke();
    }
    #endregion
}
