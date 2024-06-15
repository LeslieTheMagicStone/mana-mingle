using Unity.Netcode;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public ulong ownerId { get; private set; }
    [SerializeField] private Side side;
    [SerializeField] private int damage;
    [SerializeField] private bool stayDamage;
    [SerializeField] private float damageInterval;
    private float lastDamageTime;


    public void SetOwnerId(ulong ownerId)
    {
        this.side = Side.Player;
        this.ownerId = ownerId;
    }

    void OnTriggerEnter(Collider other)
    {
        if (stayDamage) return;
        if (other.TryGetComponent(out Damageable damageable))
        {
            if (damageable.side != side || (damageable.side == Side.Player && damageable.ownerId != ownerId))
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(!stayDamage) return;
        if (Time.time - lastDamageTime < damageInterval) return;
        if (other.TryGetComponent(out Damageable damageable))
        {
            if (damageable.side != side || (damageable.side == Side.Player && damageable.ownerId != ownerId))
            {
                damageable.TakeDamage(damage);
                lastDamageTime = Time.time;
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
