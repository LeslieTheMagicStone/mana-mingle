using Unity.Netcode;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Damageable master;
    [SerializeField] private bool isPlayer;
    private Vector3 localScale;
    private float maxWidth;

    private void Awake()
    {
        if (master == null) master = GetComponentInParent<Damageable>();
        if (master == null) { Debug.LogWarning("No Damageable found on parent."); return; }

        master.OnHurt.AddListener(UpdateHealth);

        maxWidth = transform.localScale.x;
        localScale = transform.localScale;
    }

    private void Start()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        if (master == null) return;
        localScale.x = (float)master.health / master.maxHealth * maxWidth;
        transform.localScale = localScale;
    }
}
