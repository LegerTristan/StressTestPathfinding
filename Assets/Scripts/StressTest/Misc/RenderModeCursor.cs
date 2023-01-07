using System;
using UnityEngine;
using Node = Node<Cell>;

/// <summary>
/// Cursor class user in render mode
/// </summary>
public class RenderModeCursor : MonoBehaviour
{
    #region F/P
    #region CursorBase
    /// <summary>
    /// Invoked every update and give current cursor position.
    /// </summary>
    public event Action<Vector3> OnCursorUpdate = null;

    [SerializeField]
    Camera cam = null;

    [SerializeField, Range(1f, 300f)]
    float depth = 0f;

    bool IsCursorValid => cam != null;
    #endregion

    #region RenderModeCursor
    public event Action<Node> OnNodeHovered = null,
                             OnNodeUnhovered = null,
                             OnNodeUpdateWalkableState = null,
                             OnNodeSelected = null;

    Node startNode = null, endNode = null, hoveredNode = null;

    /// <summary>
    /// Used to manage cursor functionment and handle walkable state of a node.
    /// </summary>
    bool mouseWasReleased = true, canClick = true;

    public Node HoveredNode
    {
        get => hoveredNode;
        set
        {
            if (hoveredNode == value)
                return;

            if (IsHoveredNodeValid)
                OnNodeUnhovered?.Invoke(hoveredNode);

            hoveredNode = value;
            mouseWasReleased = true;

            if (IsHoveredNodeValid)
                OnNodeHovered?.Invoke(hoveredNode);
        }
    }

    public Node StartNode => startNode;

    public Node EndNode => endNode;

    bool IsHoveredNodeValid => hoveredNode != null && hoveredNode.Item != null;
    #endregion
    #endregion

    #region UnityEvents
    void Update()
    {
        if (!canClick)
            return;

        UpdateCursorPosition();
        HandleHoveredNode();
        if(Input.GetMouseButtonUp(1))
            mouseWasReleased = true;
    }
    #endregion

    #region CustomMethods
    public void Init(PathfinderMono _pathfinder, SearcherMono _searcher)
    {
        if (!_pathfinder || !_searcher)
            return;

        _pathfinder.OnPathfindingStarted += () => canClick = false;
        _searcher.OnSearchingStarted += () => canClick = false;

        _pathfinder.OnPathfindingReset += (_reachables, _visited, _results, _startNode, _endNode) => canClick = true;
        _searcher.OnSearchingReset += (_reachables, _visited, _results, _startNode) => canClick = true;
    }

    void UpdateCursorPosition()
    {
        if (!IsCursorValid)
            return;

        Vector3 _mousePos = Input.mousePosition;
        Vector3 _cursorPos = cam.ScreenToWorldPoint(new Vector3(_mousePos.x, _mousePos.y, depth));
        OnCursorUpdate?.Invoke(_cursorPos);
    }

    void HandleHoveredNode()
    {
        SetNodeSelectState();
        SetNodeWalkableState();
    }

    #region Select State
    /// <summary>
    /// Set node select state if player just pressed left mouse button.
    /// </summary>
    void SetNodeSelectState()
    {
        bool _isValid = Input.GetMouseButtonDown(0);

        if (!_isValid || !IsHoveredNodeValid || !hoveredNode.IsWalkable)
            return;

        if (hoveredNode.Item.IsSelected)
            UnselectNode();
        else
            SelectNode();

        OnNodeSelected?.Invoke(hoveredNode);
    }

    void SelectNode()
    {
        bool _set = SetNodeSelected(hoveredNode);
        hoveredNode.Item.IsSelected = _set;
    }

    void UnselectNode()
    {
        SetNodeUnselected(hoveredNode);
        hoveredNode.Item.IsSelected = false;
    }

    /// <summary>
    /// Set node as selected if possible.
    /// </summary>
    /// <param name="_node">Node to select</param>
    /// <returns>True if node was selected, else false.</returns>
    bool SetNodeSelected(Node _node)
    {
        if (startNode == null)
        {
            startNode = _node;
            return true;
        }
        else if(endNode == null)
        {
            endNode = _node;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Set a node as unselected. If node is start node, then end node became new start node 
    /// and previous start is unselected.
    /// </summary>
    /// <param name="_node">Node to unselect</param>
    void SetNodeUnselected(Node _node)
    {
        if (startNode == _node)
        {
            startNode = endNode;
            endNode = null;
        }
        if (endNode == _node)
            endNode = null;
    }
    #endregion

    #region Walkable State
    /// <summary>
    /// Set a node as unwalkable or walkable depending on previous state.
    /// Can only set walkable state if right mouse button was just pressed.
    /// </summary>
    void SetNodeWalkableState()
    {
        bool _isValid = Input.GetMouseButton(1);

        if (!_isValid || !IsHoveredNodeValid || !mouseWasReleased)
            return;

        if (hoveredNode.IsWalkable)
            SetNodeUnwalkable();
        else
            SetNodeWalkable();

        mouseWasReleased = false;
    }

    void SetNodeWalkable()
    {
        if(hoveredNode != startNode && hoveredNode != endNode)
        {
            hoveredNode.IsWalkable = true;
            OnNodeUpdateWalkableState(hoveredNode);
        }
    }

    void SetNodeUnwalkable()
    {
        if (hoveredNode != startNode && hoveredNode != endNode)
        {
            hoveredNode.IsWalkable = false;
            OnNodeUpdateWalkableState(hoveredNode);
        }
    }

    #endregion

    public void ResetNodes()
    {
        if(startNode != null && startNode.Item && endNode != null && endNode.Item)
            startNode.Item.IsSelected = endNode.Item.IsSelected = false;
        startNode = endNode = null;
    }
    #endregion
}
