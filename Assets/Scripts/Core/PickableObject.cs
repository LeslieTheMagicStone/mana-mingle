using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private Material outlinePrefab;
    private Material outline;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.materials = meshRenderer.materials.Append(outlinePrefab).ToArray();
        outline = meshRenderer.materials[^1];
        OnHighlightExit();
    }

    public void OnHighlightEnter()
    {
        outline.SetFloat("_Scale", 1.1f);
    }

    public void OnHighlightExit()
    {
        outline.SetFloat("_Scale", 1.0f);
    }

    public virtual void OnPick(PickerLogic picker) { }
}
