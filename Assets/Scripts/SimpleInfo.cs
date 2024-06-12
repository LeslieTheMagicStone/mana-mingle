using Unity.Netcode;
using UnityEngine;
using TMPro;

public class SimpleInfo : MonoBehaviour
{
    public void Start()
    {
        var text = GetComponentInChildren<TMP_Text>();
        text.text = GetComponentInParent<Damager>().ownerId.ToString();
    }
}
