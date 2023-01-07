using System.Collections;
using System.Collections.Generic;
using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Child class of Searching.
/// Apply Djikstra as searching algorithm.
/// </summary>
public class DjikstraSearch : Searching
{
    #region F/P
    /// <summary>
    /// Binary heap to sort node by their FCost value, the minimum value is extracted first 
    /// and the maximum last.
    /// </summary>
    Heap<Node> reachableNodes = null;

    public override IEnumerable ReachableNodes => reachableNodes;

    public override bool IsEnded => reachableNodes.Count == 0;
    #endregion

    public DjikstraSearch(Grid _grid, Node _startNode, int _maxRange) : base(_grid, _startNode, _maxRange)
    {
        reachableNodes = new Heap<Node>(_grid.MaxSize);
    }

    #region CustomMethods

    public override void Init()
    {
        startNode.GCost = 0;
        startNode.FCost = 0;
        reachableNodes.Clear();
        reachableNodes.Add(startNode);
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

        return _visitedNodes;
    }

    public override bool LoopAlgo(ref HashSet<Node> _visitedNodes)
    {
        List<Node> _currentNeighbours;
        Node _currentNode = reachableNodes.ExtractFirst();

        _visitedNodes.Add(_currentNode);

        if (_currentNode == null)
            return true;

        _currentNeighbours = GetAdjacentNodes(_currentNode);

        for (int i = 0; i < _currentNeighbours.Count; i++)
        {
            Node _currentNeighbour = _currentNeighbours[i];

            if (_visitedNodes.Contains(_currentNeighbour) || !_currentNeighbour.IsWalkable)
                continue;

            int _moveCost = _currentNode.GCost + ComputeHCost(_currentNode, _currentNeighbour);

            if ((!reachableNodes.Contains(_currentNeighbour) && _moveCost <= maxRange))
            {
                _currentNeighbour.GCost = _moveCost;
                _currentNeighbour.FCost = _moveCost;

                if (!reachableNodes.Contains(_currentNeighbour))
                    reachableNodes.Add(_currentNeighbour);
            }
        }

        return false;
    }
    #endregion
}
