using System.Collections;
using System.Collections.Generic;
using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Child class of Pathfinding.
/// Apply BFS as pathfinding algorithm.
/// </summary>
public class BFS : Pathfinding
{
    #region F/P
    /// <summary>
    /// Since we studying every node of a same range then wego to the next range,
    /// we use a queue to follow the FIFO rule.
    /// </summary>
    Queue<Node> reachableNodes = new Queue<Node>();

    public override IEnumerable ReachableNodes => reachableNodes;

    public override bool IsEnded => reachableNodes.Count == 0;
    #endregion

    public BFS(Grid _grid, Node _startNode, Node _endNode) : base(_grid, _startNode, _endNode) {}


    #region CustomMethods
    public override void Init()
    {
        startNode.FCost = -1;
        startNode.Previous = null;
        reachableNodes.Clear();
        reachableNodes.Enqueue(startNode);
    }

    public override IEnumerable<Node<Cell>> FullAlgo()
    {
        if (!IsValid)
            return null;

        Init();

        HashSet<Node> _visitedNodes = new HashSet<Node>();
        bool _result = false;

        while (reachableNodes.Count > 0 && !_result)
            _result = LoopAlgo(ref _visitedNodes);

        return Backtrace(endNode);
    }

    public override bool LoopAlgo(ref HashSet<Node> _visitedNodes)
    {
        List<Node> _currentNeighbours;
        Node _currentNode = reachableNodes.Dequeue();

        _visitedNodes.Add(_currentNode);

        if (_currentNode == null || _currentNode == endNode)
            return true;

        _currentNeighbours = GetAdjacentNodes(_currentNode);

        for (int i = 0; i < _currentNeighbours.Count; i++)
        {
            Node _currentNeighbour = _currentNeighbours[i];

            if (_visitedNodes.Contains(_currentNeighbour) || !_currentNeighbour.IsWalkable || 
                reachableNodes.Contains(_currentNeighbour))
                continue;

            _currentNeighbour.FCost = -1;
            _currentNeighbour.Previous = _currentNode;
            reachableNodes.Enqueue(_currentNeighbour);
        }

        return false;
    }
    #endregion
}
