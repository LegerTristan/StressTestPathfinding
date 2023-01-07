using System.Collections;
using System.Collections.Generic;
using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Child class of Searching.
/// Apply DFS / DLS as searching algorithm.
/// </summary>
public class DFSSearch : Searching
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

    public DFSSearch(Grid _grid, Node _startNode, int _maxRange) : base(_grid, _startNode, _maxRange) { }

    #region CustomMethods
    public override void Init()
    {
        startNode.GCost = 0;
        startNode.FCost = -1;
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

        while (!_result)
            _result = LoopAlgo(ref _visitedNodes);

        return _visitedNodes;
    }

    public override bool LoopAlgo(ref HashSet<Node> _visitedNodes)
    {
        List<Node> _currentNeighbours;
        Node _currentNode = reachableNodes.Pop();

        _visitedNodes.Add(_currentNode);

        if (reachableNodes.Count == 0 && _currentNode == null)
            return true;

        _currentNeighbours = GetAdjacentNodes(_currentNode);

        for (int i = 0; i < _currentNeighbours.Count; i++)
        {
            Node _currentNeighbour = _currentNeighbours[i];

            int _moveCost = _currentNode.GCost + UNWEIGHTED_COST;

            //if (_visitedNodes.Contains(_currentNeighbour) && _moveCost < _currentNeighbour.GCost)
            //    _visitedNodes.Remove(_currentNeighbour);

            if (_visitedNodes.Contains(_currentNeighbour) || !_currentNeighbour.IsWalkable)
                continue;

            if (!reachableNodes.Contains(_currentNeighbour) && _moveCost <= maxRange)
            {
                _currentNeighbour.GCost = _moveCost;
                _currentNeighbour.FCost = -1;
                reachableNodes.Push(_currentNeighbour);
            }
        }

        return false;
    }
    #endregion
}
