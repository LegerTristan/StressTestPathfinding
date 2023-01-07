using System.Collections;
using System.Collections.Generic;
using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Child class of Pathfinding.
/// Apply A* pathfinding algorithm.
/// </summary>
public sealed class AStar : Pathfinding
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

    #region Constructor
    public AStar(Grid _grid, Node _startNode, Node _endNode) : base(_grid, _startNode, _endNode)
    {
        reachableNodes = new Heap<Node>(_grid.MaxSize);
    }
    #endregion

    #region CustomMethods
    public override void Init()
    {
        startNode.GCost = 0;
        startNode.HCost = ComputeHCost(startNode, endNode);
        startNode.FCost = startNode.GCost + startNode.HCost;
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

        while(reachableNodes.Count > 0 && !_result)
            _result = LoopAlgo(ref _visitedNodes);

        return Backtrace(endNode);
    }

    public override bool LoopAlgo(ref HashSet<Node> _visitedNodes)
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

            int _moveCost = _currentNode.GCost + ComputeHCost(_currentNode, _currentNeighbour);

            // We add this node to reachables nodes if it wasn't visited before or if current moveCost is better
            // than its previous GCost.
            // Then we add it to reachables nodes, and set that its previous node is the current node.
            // Also, we update every cost of the node to match its new move cost
            // and sort it properly in the binary heap.
            if (_moveCost < _currentNeighbour.GCost || !reachableNodes.Contains(_currentNeighbour))
            {
                _currentNeighbour.GCost = _moveCost;
                _currentNeighbour.HCost = ComputeHCost(_currentNeighbour, endNode);
                _currentNeighbour.FCost = _currentNeighbour.GCost + _currentNeighbour.HCost;
                _currentNeighbour.Previous = _currentNode;

                if (!reachableNodes.Contains(_currentNeighbour))
                    reachableNodes.Add(_currentNeighbour);
            }
        }

        return false;
    }
    #endregion
}
