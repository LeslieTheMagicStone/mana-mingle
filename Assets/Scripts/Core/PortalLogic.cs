using UnityEngine;

public class PortalLogic : MonoBehaviour
{
    [SerializeField] private Transform destination;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerLogic player))
        {
            player.Teleport(destination);
        }
    }
}
