using System.Linq;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] private Material outlinePrefab;
    private Material outline;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.materials = meshRenderer.materials.Append(outlinePrefab).ToArray();
        outline = meshRenderer.materials[^1];
        OnMouseExit();
    }

    private void OnMouseEnter()
    {
        outline.SetFloat("_Scale", 1.1f);
    }

    private void OnMouseExit()
    {
        outline.SetFloat("_Scale", 1.0f);
    }

    private void OnMouseDown()
    {
        outline.SetFloat("_Scale", 1.2f);
    }
}
