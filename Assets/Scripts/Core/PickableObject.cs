using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PickableObject : MonoBehaviour
{
    protected MeshRenderer targetRenderer;
    private Material outline;

    protected virtual void Awake()
    {
        var origOutline = Resources.Load<Material>("Materials/Outline");
        if (targetRenderer == null) targetRenderer = GetComponent<MeshRenderer>();
        targetRenderer.materials = targetRenderer.materials.Append(origOutline).ToArray();
        outline = targetRenderer.materials[^1];
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
