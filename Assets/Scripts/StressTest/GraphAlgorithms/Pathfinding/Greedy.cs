using System.Collections;
using System.Collections.Generic;
using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Child class of Pathfinding.
/// Apply Greedy as pathfinding algorithm.
/// </summary>
public sealed class Greedy : Pathfinding
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

    public Greedy(Grid _grid, Node _startNode, Node _endNode) : base(_grid, _startNode, _endNode) 
    {
        reachableNodes = new Heap<Node>(_grid.MaxSize);
    }

    #region CustomMethods
    public override void Init()
    {
        startNode.HCost = ComputeHCost(startNode, endNode);
        startNode.FCost = startNode.HCost;
        startNode.Previous = null;
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

        return Backtrace(endNode);
    }

    public override bool LoopAlgo(ref HashSet<Node> _visitedNodes)
    {
        {
            List<Node> _currentNeighbours;

            Node _currentNode = reachableNodes.ExtractFirst();

            _visitedNodes.Add(_currentNode);

            if (_currentNode == null || _currentNode == endNode)
                return true;

            _currentNeighbours = GetAdjacentNodes(_currentNode);

            for (int i = 0; i < _currentNeighbours.Count; i++)
            {
                Node _currentNeighbour = _currentNeighbours[i];

                if (_visitedNodes.Contains(_currentNeighbour) || !_currentNeighbour.IsWalkable)
                    continue;

                int _distFromEnd = ComputeHCost(_currentNeighbour, endNode);

                if (_distFromEnd < _currentNeighbour.HCost || !reachableNodes.Contains(_currentNeighbour))
                {
                    _currentNeighbour.HCost = _distFromEnd;
                    _currentNeighbour.FCost = _currentNeighbour.HCost;
                    _currentNeighbour.Previous = _currentNode;

                    if (!reachableNodes.Contains(_currentNeighbour))
                        reachableNodes.Add(_currentNeighbour);
                }
            }

            return false;
        }
    }
    #endregion
}
