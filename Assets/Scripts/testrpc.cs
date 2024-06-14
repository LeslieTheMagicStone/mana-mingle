using Unity.Netcode;
using UnityEngine;

public class testrpc : NetworkBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            Des();
        }
    }

    private void Des()
    {
        print("Des called once");

        DesServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DesServerRpc()
    {
        print("Des server rpc called once");

        GetComponent<NetworkObject>().Despawn();
    }
}
