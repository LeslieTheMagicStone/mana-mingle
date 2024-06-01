using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] Side side;

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;
        if (other.attachedRigidbody.TryGetComponent(out Damageable damageable))
        {
            if (damageable.side != side)
            {
                damageable.TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && !GameManager.instance.debugMode) return;

        Gizmos.color = new(1, 0, 0, 0.6f);
        Gizmos.DrawSphere(transform.position, transform.localScale.x / 2);
    }
}
