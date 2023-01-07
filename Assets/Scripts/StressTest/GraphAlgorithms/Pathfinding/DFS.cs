using System.Collections;
using System.Collections.Generic;
using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Child class of Pathfinding.
/// Apply DFS as pathfinding algorithm.
/// </summary>
public class DFS : Pathfinding
{
    #region F/P
    /// <summary>
    /// In order to treat every node of a branch before going to the next, we need to follow the LIFO rule,
    /// so we use a stack.
    /// </summary>
    Stack<Node> reachableNodes = new Stack<Node>();

    public override IEnumerable ReachableNodes => reachableNodes;

    public override bool IsEnded => reachableNodes.Count == 0;
    #endregion

    public DFS(Grid _grid, Node _startNode, Node _endNode) : base(_grid, _startNode, _endNode) { }

    #region CustomMethods

    public override void Init()
    {
        startNode.FCost = -1;
        startNode.Previous = null;
        reachableNodes.Clear();
        reachableNodes.Push(startNode);
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
        Node _currentNode = reachableNodes.Pop();

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
            reachableNodes.Push(_currentNeighbour);
        }

        return false;
    }
    #endregion
}
