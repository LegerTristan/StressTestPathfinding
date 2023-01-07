using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Node = Node<Cell>;

/// <summary>
/// Every graph algorithm type.
/// 
/// Search : Searching algorithm, more commonly called "Blind search" algorithm.
/// In this case, we used an algorithm with a precise range and we get all nodes that are reachable
/// in this range. 
/// Move cost is simply based on distance from start node for non-weighted graph and
/// on cost of the current node for weighted graph.
/// 
/// Destination : When we know the destination node, we can used a pathfinding algorithm such as
/// Djikstra or A*.
/// Pathfinding algorithm can also be used on a non-weighted graph and algoritm that go with like BFS.
/// </summary>
public enum AlgoType
{
    SEARCH = 0,
    DESTINATION = 1,
}

/// <summary>
/// MonoBehaviour class for every algo types.
/// Contains logic for managing algorithm progress, and broadcast progress through delegates.
/// </summary>
public abstract class GraphAlgoMono : MonoBehaviour
{
    #region F/P

    /// <summary>
    /// Node that has been marked as visited, meaning that we don't need to check them twice.
    /// </summary>
    protected HashSet<Node> visitedNodes = new HashSet<Node>();

    /// <summary>
    /// For pathfinding algorithm, represents the path to end node.
    /// </summary>
    protected List<Node> resultNodes = new List<Node>();

    #region Timer
    protected WaitForSeconds clickWait = new WaitForSeconds(0.1f);

    protected WaitForSeconds timerWait = null;

    /// <summary>
    /// A short wait time to prevent clickWait to being triggered in loop.
    /// </summary>
    protected WaitUntil clickUntilWait = null;

    [SerializeField, Range(0.1f, 10f)]
    protected float waitTime = 1.0f;

    [SerializeField]
    protected bool useTimer = true, useStepByStep = true;

    /// <summary>
    /// Getter/Setter for wait time between each algorithm update.
    /// </summary>
    public float WaitTime
    {
        get => waitTime;
        set
        {
            waitTime = value;
            timerWait = new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// Getter / Setter for useTimer boolean.
    /// </summary>
    public bool UseTimer
    {
        get => useTimer;
        set => useTimer = value;
    }

    /// <summary>
    /// Getter / Setter for useTimer boolean.
    /// </summary>
    public bool UseStepByStep
    {
        get => useStepByStep;
        set => useStepByStep = value;
    }
    #endregion

    #region Chrono
    [SerializeField]
    protected ChronoMetrics chronoMetrics = ChronoMetrics.SECONDS;

    public Chronometer Chrono { get; set; } = null;

    [SerializeField]
    protected bool useChrono = true;
    #endregion

    /// <summary>
    /// Number of execution of the same algorithm we want.
    /// Useful for getting an average execution time of an algorithm.
    /// </summary>
    [SerializeField, Range(1, 100)]
    protected int algoExecutionCount = 5;

    protected int currentNbrAlgoExecution = 0;

    public int AlgoExecutionCount
    {
        get => algoExecutionCount;
    }

    protected bool isProcessing = false;
    #endregion

    void Awake() => Chrono = new Chronometer(chronoMetrics);

    #region CustomMethods
    public virtual void Init()
    {
        timerWait = new WaitForSeconds(waitTime);
        clickUntilWait = new WaitUntil(() => Input.GetMouseButtonDown(0));
    }

    #region Algorithm

    /// <summary>
    /// Launch graphs algorithm in performance mode (without coroutine to manipulate progress)
    /// </summary>
    protected abstract void PlayAlgorithm();

    public virtual void ResetAlgorithm()
    {
        visitedNodes.Clear();
        resultNodes.Clear();
        StopAllCoroutines();
        isProcessing = false;
    }
    #endregion

    #region Coroutine
    /// <summary>
    /// Launch graphs algorithm in render mode (with coroutine to manipulate progress)
    /// </summary>
    protected void PlayAlgorithmCoroutine()
    {
        currentNbrAlgoExecution = 0;
        StartCoroutine(AlgorithmCoroutine().GetEnumerator());
    }

    /// <summary>
    /// Main loop of graphs algorithm.
    /// The coroutine allows us to manipulate algorithm progress.
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable AlgorithmCoroutine();

    protected abstract void RestartAlgorithmCoroutine();

    #endregion
    #endregion

}
