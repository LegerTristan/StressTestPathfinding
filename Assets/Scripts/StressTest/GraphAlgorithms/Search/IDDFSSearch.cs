using System.Collections;
using System.Collections.Generic;
using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Child class of Searching.
/// Apply IDDFS as searching algorithm.
/// </summary>
public class IDDFSSearch : Searching
{
    #region F/P
    /// <summary>
    /// In order to treat every node of a branch before going to the next in the DLS part,
    /// we need to follow the LIFO rule, so we use a stack.
    /// </summary>
    Stack<Node> reachableNodes = new Stack<Node>();

    int currentRange = 1;

    public override IEnumerable ReachableNodes => reachableNodes;

    public override bool IsEnded => reachableNodes.Count == 0 && currentRange > maxRange;
    #endregion

    public IDDFSSearch(Grid _grid, Node _startNode, int _maxRange) : base(_grid, _startNode, _maxRange) { }

    #region F/P
    public override void Init()
    {
        startNode.GCost = 0;
        startNode.FCost = -1;
        currentRange = 1;
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

        return _visitedNodes;
    }

    public override bool LoopAlgo(ref HashSet<Node> _visitedNodes)
    {
        if (reachableNodes.Count == 0)
        {
            NextRange(ref _visitedNodes);
            return currentRange > maxRange;
        }

        List<Node> _currentNeighbours;
        Node _currentNode = reachableNodes.Pop();

        _visitedNodes.Add(_currentNode);
        _currentNeighbours = GetAdjacentNodes(_currentNode);

        for (int i = 0; i < _currentNeighbours.Count; i++)
        {
            Node _currentNeighbour = _currentNeighbours[i];

            int _moveCost = _currentNode.GCost + UNWEIGHTED_COST;

            if (_visitedNodes.Contains(_currentNeighbour) || !_currentNeighbour.IsWalkable)
                continue;

            if (!reachableNodes.Contains(_currentNeighbour) && _moveCost <= currentRange)
            {
                _currentNeighbour.GCost = _moveCost;
                _currentNeighbour.FCost = -1;
                reachableNodes.Push(_currentNeighbour);
            }
        }

        return false;
    }

    /// <summary>
    /// Increment currentRange and clear visited nodes then push startNode in reachableNodes
    /// to restart DLS part of the algorithm
    /// </summary>
    /// <param name="_visitedNodes">Nodes that was currently visited in algorithm</param>
    void NextRange(ref HashSet<Node> _visitedNodes)
    {
        currentRange++;
        if(currentRange <= maxRange)
        {
            _visitedNodes.Clear();
            reachableNodes.Push(startNode);
        }
    }
    #endregion
}
