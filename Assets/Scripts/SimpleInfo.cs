using Unity.Netcode;
using UnityEngine;
using TMPro;

public class SimpleInfo : MonoBehaviour
{
    TMP_Text text;
    public void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {

        text.text = GetComponentInParent<NetworkObject>().OwnerClientId.ToString();
    }
}
