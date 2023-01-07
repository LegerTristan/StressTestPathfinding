using UnityEngine;

/// <summary>
/// Represents a part of the grid with a mesh 
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cell : MonoBehaviour
{
    #region F/P
    MeshRenderer meshRenderer = null;

    public TextMesh TextMesh { get; set; } = null;

    public bool IsSelected { get; set; } = false;

    public bool HasTextMesh => TextMesh;
    #endregion

    #region UnityEvents
    private void Start() => meshRenderer = GetComponent<MeshRenderer>();
    #endregion

    #region CustomMethods
    public void SetColor(Color _color)
    {
        if(meshRenderer)
            meshRenderer.material.color = _color;
    }
    #endregion
}
