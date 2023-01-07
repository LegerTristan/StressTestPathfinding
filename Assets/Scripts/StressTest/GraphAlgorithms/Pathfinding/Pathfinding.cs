using System.Collections.Generic;
using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Base class for pathfinding algorithm.
/// Contains backtrace function.
/// </summary>
public abstract class Pathfinding : GraphAlgorithm
{
    #region F/P
    protected Node endNode = null;

    public Node EndNode => endNode;

    public override bool IsValid => base.IsValid && endNode != null;
    #endregion

    public Pathfinding(Grid _grid, Node _startNode, Node _endNode) : base(_grid, _startNode) 
    {
        endNode = _endNode;
    }

    public List<Node> Backtrace(Node _endNode)
    {
        List<Node> _resultNodes = new List<Node>();
        Node _currentNode = _endNode;

        while (_currentNode != null)
        {
            _resultNodes.Add(_currentNode);
            _currentNode = _currentNode.Previous;
        }

        _resultNodes.Reverse();
        return _resultNodes;
    }
}
