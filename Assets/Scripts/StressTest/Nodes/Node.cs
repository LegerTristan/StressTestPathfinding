using UnityEngine;

/// <summary>
/// Base node class.
/// Inherit from interface IHeapItem in order to work with BinaryHeap.
/// Contains world position of the node, an item, and members used for pathfinding (cost, previous node)
/// </summary>
/// <typeparam name="TItem">Item contained by the node, template type must inherit of MonoBehaviour or is a MonoBehaviour.</typeparam>
public class Node<TItem> : IHeapItem<Node<TItem>> where TItem : MonoBehaviour
{
    #region F/P

    #region Node
    /// <summary>
    /// Position of the node in world space.
    /// </summary>
    protected Vector3 worldPos = Vector3.zero;

    /// <summary>
    /// Index of the node in the grid.
    /// </summary>
    int x = 0, y = 0;

    public Vector3 WorldPos => worldPos;

    public int X => x;

    public int Y => y;

    /// <summary>
    /// Heap index of the node.
    /// Useful for managing it in a binary heap.
    /// </summary>
    public int HeapIndex { get; set; } = 0;
    #endregion

    #region Pathfinding
    public Node<TItem> Previous { get; set; } = null;

    /// <summary>
    /// Current global cost.
    /// Generally this i the distance from start node to this node.
    /// </summary>
    public int GCost { get; set; } = int.MaxValue;

    /// <summary>
    /// Current heuristic cost.
    /// Generally this is the distance from this node to end node.
    /// </summary>
    public int HCost { get; set; } = int.MaxValue;

    /// <summary>
    /// Final cost to move to this node.
    /// Generally, that is an addition of the GCost and the HCost.
    /// </summary>
    public int FCost { get; set; } = int.MaxValue;

    public bool IsWalkable { get; set; } = true;

    #endregion

    /// <summary>
    /// An item that is binded to this node.
    /// </summary>
    TItem item = default(TItem);

    public TItem Item => item;

    #endregion

    #region CustomMethods
    /// <summary>
    /// Main constructor of a node.
    /// Also, set item position in world space and set its name.
    /// </summary>
    /// <param name="_item">Item that is binded to this node</param>
    /// <param name="_worldPos">World position of the node.</param>
    /// <param name="_x">X index of the node in the grid.</param>
    /// <param name="_y">Y index of the node in the grid.</param>
    public Node(TItem _item, Vector3 _worldPos, int _x, int _y)
    {
        worldPos = _worldPos;
        x = _x;
        y = _y;

        item = _item;

        if (!item)
            return;

        item.name = $"Node_{x}{y}";
        item.transform.position = worldPos;
    }

    public virtual void ResetPathfindingMembers()
    {
        FCost = GCost = HCost = 0;
        Previous = null;
    }

    /// <summary>
    /// Remove item present in the node.
    /// </summary>
    public void Clear()
    {
        if (Item)
            Object.Destroy(Item.gameObject);
    }


    /// <summary>
    /// Compare two nodes by their FCost value and , if FCost are equals,
    /// by their HCost.
    /// </summary>
    /// <param name="_other"></param>
    /// <returns>A value that can be :
    /// 1 : Meaning that current node FCost is inferior to other node FCost.
    /// 0 : If current node FCost AND HCost are equals to FCost and HCost of the other node.
    /// -1 : Current node FCost is superior to other node FCost.</returns>
    public int CompareTo(Node<TItem> _other)
    {
        int _result = FCost.CompareTo(_other.FCost);

        if(_result == 0)
            _result = HCost.CompareTo(_other.HCost);

        return _result;
    }
    #endregion
}
