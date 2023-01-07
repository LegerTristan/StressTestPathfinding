using UnityEngine;

/// <summary>
/// Manager of the scene.
/// Contains every main class of the scene.
/// </summary>
public class GraphAlgoManager : SingletonTemplate<GraphAlgoManager>
{
    #region F/P
    [SerializeField]
    PathfinderMono pathfinder = null;

    [SerializeField]
    SearcherMono searcher = null;

    [SerializeField]
    RenderModeCamera cam = null;

    GraphAlgoCreator creator = null;

    [SerializeField]
    RenderModeUI renderUI = null;

    [SerializeField]
    PerformanceModeUI performanceUI = null;

    [SerializeField]
    RenderModeCursor cursor = null;

    [SerializeField]
    GridMono gridMono = null;

    [SerializeField]
    NodesDrawer drawer = null;

    bool IsManagerValid => pathfinder && searcher && cam && renderUI && cursor && drawer;
    #endregion

    #region UnityEvents
    void Start() => Init();
    #endregion

    #region CustomMethods
    void Init()
    {
        if (!IsManagerValid)
            return;

        pathfinder.Init();
        searcher.Init();
        cursor.Init(pathfinder, searcher);
        gridMono.Init(cursor);
        cam.Init(gridMono);
        drawer.Init(gridMono, pathfinder, searcher, cursor);
        renderUI.Init(gridMono, pathfinder, searcher, cursor);
        performanceUI.Init(gridMono, pathfinder, searcher);
        creator = new GraphAlgoCreator(gridMono);

        SetBindings();
    }

    void SetBindings()
    {
        renderUI.OnRenderModeHide += () => performanceUI.ResumePerformanceMode(renderUI.AlgoType);

        renderUI.OnStartPathfindingClicked += (_algo, _executionCount, _range) =>
            pathfinder.StartPathfinding(creator.CreatePathfindingAlgorithm(_algo, cursor.StartNode, cursor.EndNode,
                _range), _executionCount);

        renderUI.OnStartSearchingClicked += (_algo, _executionCount, _range) =>
            searcher.StartSearching(creator.CreateSearchingAlgorithm(_algo, cursor.StartNode, _range),
                _executionCount);

        performanceUI.OnPerformanceModeHide += () => renderUI.ResumeRenderMode(performanceUI.AlgoType);

        performanceUI.OnStartPathfindingClicked += (_algo, _startNode, _endNode, _executionCount, _range) =>
            pathfinder.StartPathfinding(creator.CreatePathfindingAlgorithm(_algo, _startNode, _endNode,
                _range),  _executionCount, false);

        performanceUI.OnStartSearchingClicked += (_algo, _startNode, _executionCount, _range) =>
            searcher.StartSearching(creator.CreateSearchingAlgorithm(_algo, cursor.StartNode, _range),
                _executionCount, false);
    }
    #endregion
}
