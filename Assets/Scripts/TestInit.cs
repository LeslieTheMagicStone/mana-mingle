using Unity.Netcode;
using UnityEngine;
using WarriorAnimsFREE;

public class TestInit : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        ulong id = NetworkManager.LocalClientId;
        transform.GetChild(0).position = new Vector3(id * 2, 0, 0);
        // transform.GetChild(0).gameObject.SetActive(false);
        if (!IsOwner)
        {
            transform.GetChild(0).GetComponent<GUIControls>().enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
