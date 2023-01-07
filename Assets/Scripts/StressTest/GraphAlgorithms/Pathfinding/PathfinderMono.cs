using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Node = Node<Cell>;

/// <summary>
/// Enumertion of pathfinding algorihm type.
/// </summary>
public enum PathfindingAlgo
{
    ASTAR = 0,
    DJIKSTRA = 1,
    GREEDY = 2,
    BFS = 3,
    DFS = 4,
    IDDFS = 5,
}

/// <summary>
/// MonoBehaviour class to handle pathfinding algorithms.
/// </summary>
public sealed class PathfinderMono : GraphAlgoMono
{
    #region F/P
    public event Action OnPathfindingStarted = null,
                        OnCurrentPathfindingEnded = null;

    public event Action<IEnumerable, IEnumerable<Node>, List<Node>> OnPathfindingUpdated = null;
    public event Action<IEnumerable, IEnumerable<Node>, List<Node>, Node, Node> OnPathfindingEnded = null,
                                                                                OnPathfindingReset = null;

    Pathfinding pathfinding = null;

    public PathfindingAlgo Algo { get; set; } = PathfindingAlgo.ASTAR;
    #endregion

    #region CustomMethods

    public override void Init()
    {
        base.Init();
        OnPathfindingStarted += () =>
        {
            if (useChrono)
                Chrono.StartChrono();
        };

        OnCurrentPathfindingEnded += () =>
        {
            if (useChrono)
                Chrono.NextTurnChrono();
        };

        OnPathfindingEnded += (_reachables, _visited, _results, _startNode, _endNode) =>
        {
            if (useChrono)
                Chrono.EndChrono();
        };
    }

    #region Pathfinding

    /// <summary>
    /// Start a new pathfinding algorithm.
    /// </summary>
    /// <param name="_pathfindingAlgo">Pathfinding algorithm to start.</param>
    /// <param name="_executionCount">Number of times we want to execute this algorithm.</param>
    /// <param name="_useCoroutine">Do we use coroutine to manage the progress or not.</param>
    public void StartPathfinding(Pathfinding _pathfindingAlgo, int _executionCount, bool _useCoroutine = true)
    {
        if (isProcessing || _pathfindingAlgo == null || !_pathfindingAlgo.IsValid)
            return;

        algoExecutionCount = _executionCount;
        pathfinding = _pathfindingAlgo;
        OnPathfindingStarted?.Invoke();

        if (_useCoroutine)
        {
            OnCurrentPathfindingEnded += RestartAlgorithmCoroutine;
            PlayAlgorithmCoroutine();
        }
        else
            PlayAlgorithm();
    }

    protected override void PlayAlgorithm()
    {
        currentNbrAlgoExecution = 0;
        OnCurrentPathfindingEnded -= RestartAlgorithmCoroutine;

        while (currentNbrAlgoExecution < algoExecutionCount)
        {
            pathfinding.FullAlgo();
            currentNbrAlgoExecution++;
            OnCurrentPathfindingEnded?.Invoke();
        }

        OnPathfindingEnded?.Invoke(pathfinding.ReachableNodes, visitedNodes, resultNodes, 
            pathfinding.StartNode, pathfinding.EndNode);
    }

    public override void ResetAlgorithm()
    {
        if (pathfinding == null)
            return;

        OnPathfindingReset?.Invoke(pathfinding.ReachableNodes, visitedNodes, resultNodes, 
            pathfinding.StartNode, pathfinding.EndNode);
        pathfinding = null;
        base.ResetAlgorithm();
    }
    #endregion

    #region Coroutine

    protected override IEnumerable AlgorithmCoroutine()
    {
        isProcessing = true;
        visitedNodes.Clear();
        resultNodes.Clear();
        pathfinding.Init();
        bool _result = false;

        while (!pathfinding.IsEnded && !_result)
        {
            _result = pathfinding.LoopAlgo(ref visitedNodes);
            if (!_result)
            {
                if(useStepByStep || useTimer)
                {
                    OnPathfindingUpdated?.Invoke(pathfinding.ReachableNodes, visitedNodes, resultNodes);

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
        resultNodes = pathfinding.Backtrace(pathfinding.EndNode);
        OnCurrentPathfindingEnded?.Invoke();
    }

    protected override void RestartAlgorithmCoroutine()
    {
        if (currentNbrAlgoExecution < algoExecutionCount)
            StartCoroutine(AlgorithmCoroutine().GetEnumerator());
        else
            OnPathfindingEnded?.Invoke(pathfinding.ReachableNodes, visitedNodes, resultNodes, pathfinding.StartNode, pathfinding.EndNode);
    }
    #endregion
    #endregion

}
