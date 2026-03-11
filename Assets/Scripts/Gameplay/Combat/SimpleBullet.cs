using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    private void Update()
    {
        transform.position += transform.forward * 10f * Time.deltaTime;
    }
}
