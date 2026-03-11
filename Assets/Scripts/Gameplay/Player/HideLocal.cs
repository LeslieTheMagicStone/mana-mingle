using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class HideLocal : NetworkBehaviour
{
    private Material origMat;
    public override void OnNetworkSpawn()
    {
        origMat = GetComponent<Renderer>().material;
        if (IsLocalPlayer)
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            Material mat = Resources.Load<Material>("Materials/ShadowsOnly");
            renderer.material = mat;
        }
    }

    public void Restore()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.shadowCastingMode = ShadowCastingMode.On;
        renderer.material = origMat;
    }
}
