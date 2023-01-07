using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Base class for every graph algorithm
/// Contains common graph algorithm functions such as get neighbours.
/// </summary>
public abstract class GraphAlgorithm
{
    public const int COST_STRAIGHT_MOVE = 10, COST_DIAGONAL_MOVE = 14, UNWEIGHTED_COST = 1;

    /// <summary>
    /// Grid to used for algorithm
    /// </summary>
    protected Grid grid = null;

    /// <summary>
    /// Start node of the algorithm
    /// </summary>
    protected Node startNode = null;

    public Node StartNode => startNode;

    /// <summary>
    /// Enumeration of all nodes that are reachables by this algorithm but which was not visited
    /// </summary>
    public abstract IEnumerable ReachableNodes { get; }

    /// <summary>
    /// Tell if the algorithm process end or not
    /// </summary>
    public abstract bool IsEnded { get; }

    public virtual bool IsValid => grid != null && startNode != null;

    public GraphAlgorithm(Grid _grid, Node _startNode)
    {
        grid = _grid;
        startNode = _startNode;
    }

    /// <summary>
    /// Init part of the algorithm
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// Main loop part of the algorithm.
    /// Take a collection of nodes that was already visited.
    /// </summary>
    /// <param name="_visitedNodes">Collection of nodes which was already visited</param>
    /// <returns>True if algorithm has ended, else false.</returns>
    public abstract bool LoopAlgo(ref HashSet<Node> _visitedNodes);

    /// <summary>
    /// Play algorithm is his entirety.
    /// </summary>
    /// <returns>A collection of visited nodes or path for going to destination.</returns>
    public abstract IEnumerable<Node> FullAlgo();

    /// <summary>
    /// Get neighbours node of a specific node.
    /// </summary>
    /// <param name="_node">Node to get neighbours</param>
    /// <returns>A list of nodes that are neighbours of node passed in parameters.</returns>
    protected List<Node> GetAdjacentNodes(Node _node)
    {
        List<Node> _adjacentNodes = new List<Node>();
        if (_node == null)
            return _adjacentNodes;

        for (int i = -1; i < 2; ++i)
        {
            for (int j = -1; j < 2; ++j)
            {
                if (i == 0 && j == 0)
                    continue;
                int _posX = i + _node.X, _posY = j + _node.Y;

                if (_posX < 0 || _posX >= grid.Row || _posY < 0 || _posY >= grid.Column)
                    continue;
                _adjacentNodes.Add(grid.GridArray[_posX, _posY]);
            }
        }

        return _adjacentNodes;
    }

    /// <summary>
    /// Compute heuristic cost of node A to node B.
    /// </summary>
    /// <param name="_nodeA">First node to compute</param>
    /// <param name="_nodeB">Second node to compute</param>
    /// <returns>Heuristic cost of node A to node B</returns>
    protected int ComputeHCost(Node _nodeA, Node _nodeB)
    {
        // Grid distance, can also sed Manhattan Distance for simplicity but lower accurate results
        // Or Euclidean Distance

        if (_nodeA == null || _nodeB == null)
            return int.MaxValue;

        int _distX = Mathf.Abs(_nodeA.X - _nodeB.X);
        int _distY = Mathf.Abs(_nodeA.Y - _nodeB.Y);

        return _distX > _distY ? (COST_DIAGONAL_MOVE * _distY) + (COST_STRAIGHT_MOVE * (_distX - _distY)) :
            (COST_DIAGONAL_MOVE * _distX) + (COST_STRAIGHT_MOVE * (_distY - _distX));
    }

    /// <summary>
    /// Compute heuristic cost of node A to node B with unweighted cost (means that cost applied is constantly
    /// equals of UNWEIGHTED_COST, no matter of the diagonal or transversal).
    /// </summary>
    /// <param name="_nodeA">First node to compute</param>
    /// <param name="_nodeB">Second node to compute</param>
    /// <returns>Heuristic cost of node A to node B</returns>
    protected int ComputeUnweightedCost(Node _nodeA, Node _nodeB)
    {
        if (_nodeA == null || _nodeB == null)
            return int.MaxValue;

        int _distX = Mathf.Abs(_nodeA.X - _nodeB.X);
        int _distY = Mathf.Abs(_nodeA.Y - _nodeB.Y);

        return _distX > _distY ? _distX * UNWEIGHTED_COST : _distY * UNWEIGHTED_COST;
    }
}
