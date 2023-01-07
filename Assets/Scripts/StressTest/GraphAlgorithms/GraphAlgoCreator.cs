using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Node = Node<Cell>;

/// <summary>
/// Creation class for every algorithms
/// </summary>
public class GraphAlgoCreator
{
    /// <summary>
    /// MonoBehaviour that contains the grid used for graph algorithms
    /// </summary>
    GridMono grid = null;

    public GraphAlgoCreator(GridMono _grid)
    {
        grid = _grid;
    }

    /// <summary>
    /// Create a new pathfinding algorithm based on pathfinding enum passed in parameters.
    /// </summary>
    /// <param name="_algo">Type of pathfinding algo to create.</param>
    /// <param name="_startNode">Start node of the algorithm</param>
    /// <param name="_endNode">End node of the algorithm</param>
    /// <param name="_maxRange">Max range from the start node</param>
    /// <returns>A new pathfinding algorithm</returns>
    public Pathfinding CreatePathfindingAlgorithm(PathfindingAlgo _algo, Node _startNode, Node _endNode, 
        int _maxRange)
    {
        if (grid.Grid == null)
            return null;

        switch (_algo)
        {
            case PathfindingAlgo.ASTAR:
            default:
                return new AStar(grid.Grid, _startNode, _endNode);
            case PathfindingAlgo.DJIKSTRA:
                return new Djikstra(grid.Grid, _startNode, _endNode);
            case PathfindingAlgo.GREEDY:
                return new Greedy(grid.Grid, _startNode, _endNode);
            case PathfindingAlgo.BFS:
                return new BFS(grid.Grid, _startNode, _endNode);
            case PathfindingAlgo.DFS:
                return new DFS(grid.Grid, _startNode, _endNode);
            case PathfindingAlgo.IDDFS:
                return new IDDFS(grid.Grid, _startNode, _endNode, _maxRange);
        }
    }

    /// <summary>
    /// Create a new searching algorithm based on searching enum passed in parameters.
    /// </summary>
    /// <param name="_algo">Type of searching algo to create.</param>
    /// <param name="_startNode">Start node of the algorithm</param>
    /// <param name="_maxRange">Max range from the start node</param>
    /// <returns>A new searching algorithm</returns>
    public Searching CreateSearchingAlgorithm(SearchingAlgo _algo, Node _startNode, int _maxRange)
    {
        if (grid.Grid == null)
            return null;

        switch (_algo)
        {
            case SearchingAlgo.DFS:
            default:
                return new DFSSearch(grid.Grid, _startNode, _maxRange);
            case SearchingAlgo.BFS:
                return new BFSSearch(grid.Grid, _startNode, _maxRange);
            case SearchingAlgo.DJIKSTRA:
                return new DjikstraSearch(grid.Grid, _startNode, _maxRange);
            case SearchingAlgo.IDDFS:
                return new IDDFSSearch(grid.Grid, _startNode, _maxRange);
        }
    }
}
