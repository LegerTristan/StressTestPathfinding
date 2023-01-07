using Grid = Grid<Cell>;
using Node = Node<Cell>;

/// <summary>
/// Base class for searching algorithm.
/// </summary>
public abstract class Searching : GraphAlgorithm
{
    protected int maxRange = int.MaxValue;

    public Searching(Grid _grid, Node _startNode, int _maxRange) : base(_grid, _startNode) 
    {
        maxRange = _maxRange;
    }
}
