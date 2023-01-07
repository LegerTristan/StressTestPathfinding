using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Child class of Searching.
/// Apply BFS as searching algorithm.
/// </summary>
public class BFSSearch : Searching
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

    public BFSSearch(Grid _grid, Node _startNode, int _maxRange) : base(_grid, _startNode, _maxRange) { }

    #region CustomMethods
    public override void Init()
    {
        startNode.GCost = 0;
        startNode.FCost = -1;
        reachableNodes.Clear();
        reachableNodes.Enqueue(startNode);
    }

    public override IEnumerable<Node<Cell>> FullAlgo()
    {
        if (startNode == null)
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
        Node _currentNode = reachableNodes.Dequeue();

        _visitedNodes.Add(_currentNode);

        if (_currentNode == null)
            return true;

        _currentNeighbours = GetAdjacentNodes(_currentNode);

        for (int i = 0; i < _currentNeighbours.Count; i++)
        {
            Node _currentNeighbour = _currentNeighbours[i];

            if (_visitedNodes.Contains(_currentNeighbour) || !_currentNeighbour.IsWalkable)
                continue;

            int _moveCost = _currentNode.GCost + UNWEIGHTED_COST;

            if ((!reachableNodes.Contains(_currentNeighbour) && _moveCost <= maxRange))
            {
                _currentNeighbour.GCost = _moveCost;
                _currentNeighbour.FCost = -1;

                if(!reachableNodes.Contains(_currentNeighbour))
                    reachableNodes.Enqueue(_currentNeighbour);
            }
        }

        return false;
    }
    #endregion
}
