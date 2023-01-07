using System;
using System.Collections;
using System.Collections.Generic;
using Node = Node<Cell>;

/// <summary>
/// Enumertion of searching algorihm type.
/// </summary>
public enum SearchingAlgo
{
    DFS = 0,
    BFS = 1,
    DJIKSTRA = 2,
    IDDFS = 3,
}

/// <summary>
/// MonoBehaviour class to handle searching algorithms.
/// </summary>
public sealed class SearcherMono : GraphAlgoMono
{
    #region F/P
    public event Action OnSearchingStarted = null,
                        OnCurrentSearchingEnded = null;

    public event Action<IEnumerable, IEnumerable<Node>, List<Node>> OnSearchingUpdated = null;
    public event Action<IEnumerable, IEnumerable<Node>, List<Node>, Node> OnSearchingEnded = null,
                                                                          OnSearchingReset = null;

    Searching searching = null;

    public SearchingAlgo Algo { get; set; } = SearchingAlgo.BFS;

    int maxRange = 1;

    public int MaxRange => maxRange;
    #endregion

    #region CustomMethods

    public override void Init()
    {
        base.Init();
        OnSearchingStarted += () =>
        {
            if (useChrono)
                Chrono.StartChrono();
        };

        OnCurrentSearchingEnded += () =>
        {
            if (useChrono)
                Chrono.NextTurnChrono();
        };

        OnSearchingEnded += (_reachables, _visited, _results, _startNode) =>
        {
            if (useChrono)
                Chrono.EndChrono();
        };
    }

    #region Searching

    /// <summary>
    /// Start a new searching algorithm.
    /// </summary>
    /// <param name="_searchingAlgo">Searching algorithm to start.</param>
    /// <param name="_executionCount">Number of times we want to execute this algorithm.</param>
    /// <param name="_useCoroutine">Do we use coroutine to manage the progress or not.</param>
    public void StartSearching(Searching _searchingAlgo, int _executionCount, bool _useCoroutine = true)
    {
        if (isProcessing || _searchingAlgo == null || !_searchingAlgo.IsValid)
            return;

        algoExecutionCount = _executionCount;
        searching = _searchingAlgo;
        OnSearchingStarted?.Invoke();

        if (_useCoroutine)
        {
            OnCurrentSearchingEnded += RestartAlgorithmCoroutine;
            PlayAlgorithmCoroutine();
        }
        else
            PlayAlgorithm();

    }

    protected override void PlayAlgorithm()
    {
        currentNbrAlgoExecution = 0;
        OnCurrentSearchingEnded -= RestartAlgorithmCoroutine;

        while (currentNbrAlgoExecution < algoExecutionCount)
        {
            searching.FullAlgo();
            currentNbrAlgoExecution++;
            OnCurrentSearchingEnded?.Invoke();
        }

        OnSearchingEnded?.Invoke(searching.ReachableNodes, visitedNodes, resultNodes, searching.StartNode);
    }

    public override void ResetAlgorithm()
    {
        if (searching == null)
            return;

        OnSearchingReset?.Invoke(searching.ReachableNodes, visitedNodes, resultNodes, searching.StartNode);
        searching = null;
        base.ResetAlgorithm();
    }
    #endregion

    #region Coroutine

    protected override IEnumerable AlgorithmCoroutine()
    {
        isProcessing = true;
        visitedNodes.Clear();
        resultNodes.Clear();
        searching.Init();
        bool _result = false;

        while (!searching.IsEnded && !_result)
        {
            _result = searching.LoopAlgo(ref visitedNodes);
            if (!_result)
            {
                if(useStepByStep || useTimer)
                {
                    OnSearchingUpdated?.Invoke(searching.ReachableNodes, visitedNodes, resultNodes);

                    if (useStepByStep)
                    {
                        yield return clickUntilWait;
                        yield return clickWait;
                    }
                    else
                        yield return timerWait;
                }
            }
        }

        currentNbrAlgoExecution++;
        OnCurrentSearchingEnded?.Invoke();
    }

    protected override void RestartAlgorithmCoroutine()
    {
        if (currentNbrAlgoExecution < algoExecutionCount)
            StartCoroutine(AlgorithmCoroutine().GetEnumerator());
        else
            OnSearchingEnded?.Invoke(searching.ReachableNodes, visitedNodes, resultNodes, searching.StartNode);
    }
    #endregion
    #endregion

}
