using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PickableObject : NetworkBehaviour
{
    protected MeshRenderer targetRenderer;
    private Material outline;

    public virtual void Init()
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

    protected void Despawn()
    {
        print("On Despawn");
        DespawnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    protected void DespawnServerRpc()
    {
        print("On RPC Despawn");
        GetComponent<NetworkObject>().Despawn();
    }
}
