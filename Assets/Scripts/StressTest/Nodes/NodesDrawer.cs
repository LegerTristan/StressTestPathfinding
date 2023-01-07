using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Node = Node<Cell>;

/// <summary>
/// Draw nodes in a specific color and draw text mesh on.
/// </summary>
public class NodesDrawer : MonoBehaviour
{
    #region F/P
    [SerializeField, Header("Colors")]
    Color standardColor = Color.white;

    [SerializeField]
    Color   hoveredColor = Color.magenta, 
            selectedColor = Color.cyan,
            unwalkableColor = Color.black,
            visitedColor = Color.gray,
            extremityColor = Color.blue,    //  Extremity colors are start node and end node.
            pathColor = Color.red,
            reachableColor = Color.green;

    #endregion

    #region CustomMethods
    /// <summary>
    /// Init bindings of NodesDrawer with each parameters
    /// </summary>
    /// <param name="_gridMono">MonoBehaviour of the grid</param>
    /// <param name="_pathfinder">MonoBehaviour handler of pathfinding algorithm</param>
    /// <param name="_searcher">MonoBehaviour handler of searching algorithm</param>
    /// <param name="_cursor">Cursor to manipulate cells on the grid</param>
    public void Init(GridMono _gridMono, PathfinderMono _pathfinder, SearcherMono _searcher, 
        RenderModeCursor _cursor)
    {
        _cursor.OnNodeHovered += DrawHoveredNode;
        _cursor.OnNodeUnhovered += DrawStandardNode;
        _cursor.OnNodeUpdateWalkableState += DrawWalkableNode;
        _cursor.OnNodeSelected += DrawHoveredNode;

        _pathfinder.OnPathfindingUpdated += (_reachables, _visited, _results) =>
        {
            DrawPathfinding(_reachables, _visited, _results);
        };

        _pathfinder.OnPathfindingEnded += DrawPathfinding;
        _pathfinder.OnPathfindingReset += ClearPathfinding;

        _searcher.OnSearchingUpdated += (_reachables, _visited, _results) =>
            DrawPathfinding(_reachables, _visited, _results);

        _searcher.OnSearchingEnded += (_reachables, _visited, _results, _startNode) =>
            DrawPathfinding(_reachables, _visited, _results, _startNode);

        _searcher.OnSearchingReset += (_reachables, _visited, _results, _startNode) =>
           ClearPathfinding(_reachables, _visited, _results, _startNode);
    }

    #region Nodes
    void DrawHoveredNode(Node _node)
    {
        if (!_node.IsWalkable)
            return;

        DrawNode(_node, _node.Item.IsSelected ? selectedColor : hoveredColor);
    }

    void DrawStandardNode(Node _node)
    {
        if (!_node.IsWalkable)
            return;

        DrawNode(_node, _node.Item.IsSelected ? selectedColor : standardColor);
    }

    void DrawWalkableNode(Node _node) =>  DrawNode(_node, _node.IsWalkable ? standardColor : unwalkableColor);

    void DrawNode(Node _node, Color _color)
    {
        if (_node != null && _node.Item)
            _node.Item.SetColor(_color);
    }

    void DrawPathfinding(IEnumerable _reachableNodes, IEnumerable _visitedNodes, List<Node> _resultsNodes,
        Node _startNode = null, Node _endNode = null)
    {
        DrawNodes(_reachableNodes, reachableColor);
        DrawNodes(_visitedNodes, visitedColor);
        DrawNodes(_resultsNodes, pathColor);

        DrawFCost(_reachableNodes);
        DrawFCost(_visitedNodes);

        DrawNode(_startNode, extremityColor);
        DrawNode(_endNode, extremityColor);
    }

    void ClearPathfinding(IEnumerable _reachableNodes, IEnumerable _visitedNodes, List<Node> _resultsNodes,
    Node _startNode = null, Node _endNode = null)
    {
        HideTextMeshes(_reachableNodes);
        HideTextMeshes(_visitedNodes);

        DrawNodes(_reachableNodes, standardColor);
        DrawNodes(_visitedNodes, standardColor);
        DrawNodes(_resultsNodes, standardColor);

        DrawNode(_startNode, selectedColor);
        DrawNode(_endNode, selectedColor);
    }

    void DrawNodes(IEnumerable _nodes, Color _color)
    {
        foreach (Node _node in _nodes)
            if (_node != null && _node.Item)
                _node.Item.SetColor(_color);
    }
    #endregion

    #region FCost
    /// <summary>
    /// Draw current FCost of each nodes via a TextMesh.
    /// </summary>
    /// <param name="_nodes">Node to draw a TextMesh on.</param>
    void DrawFCost(IEnumerable _nodes)
    {
        foreach (Node _node in _nodes)
        {
            if (_node == null || !_node.Item)
                continue;

            string _toDraw = _node.FCost == -1 ? "X" : _node.FCost.ToString();

            if (_node.Item.HasTextMesh)
            {
                _node.Item.TextMesh.text = _toDraw;
                _node.Item.TextMesh.gameObject.SetActive(true);
            }
            else
                _node.Item.TextMesh = DrawTextMeshOnNode(_node.Item.transform, _toDraw, Color.black);
        }
    }

    TextMesh DrawTextMeshOnNode(Transform _parent, string _text, Color _color, int _fontSize = 30, 
        float _localScale = 0.1f, float _pitchRot = 90.0f, TextAlignment _alignment = TextAlignment.Center,
        TextAnchor _anchor = TextAnchor.MiddleCenter, FontStyle _style = FontStyle.Bold)
    {
        if (!_parent)
            return null;

        TextMesh _textMesh = new GameObject().AddComponent<TextMesh>();
        _textMesh.text = _text;
        _textMesh.alignment = _alignment;
        _textMesh.anchor = _anchor;
        _textMesh.color = _color;
        _textMesh.fontSize = _fontSize;
        _textMesh.fontStyle = _style;
        _textMesh.transform.eulerAngles = Vector3.right * _pitchRot;
        _textMesh.transform.position = _parent.position;
        _textMesh.transform.localScale = Vector3.one * _localScale;
        _textMesh.transform.SetParent(_parent);

        return _textMesh;
    }

    void HideTextMeshes(IEnumerable _nodes)
    {
        foreach(Node _node in _nodes)
            HideTextMesh(_node);
    }

    void HideTextMesh(Node _node)
    {
        if (_node != null && _node.Item && _node.Item.HasTextMesh)
            _node.Item.TextMesh.gameObject.SetActive(false);
    }
    #endregion
    #endregion
}
