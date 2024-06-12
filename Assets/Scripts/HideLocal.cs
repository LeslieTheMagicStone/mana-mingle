using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class HideLocal : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            Material mat = Resources.Load<Material>("Materials/ShadowsOnly");
            renderer.material = mat;
        }
    }
}
