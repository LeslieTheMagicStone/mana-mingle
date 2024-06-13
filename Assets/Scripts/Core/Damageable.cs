using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Unity.Netcode;

public class Damageable : NetworkBehaviour
{
    public ulong ownerId { get; private set; }
    public Side side => _side;
    public int maxHealth => _maxHealth;
    public int health => _health.Value;
    public UnityEvent onHurt;
    public UnityEvent onDeath;
    [SerializeField] private Side _side;
    [SerializeField] private int _maxHealth;
    [SerializeField] private GameObject deathParticles;
    public NetworkVariable<int> _health = new(writePerm: NetworkVariableWritePermission.Server);
    private Vector3 origPos;
    private Tween shakeTween;
    private bool isInvincible => invincibleTimer > 0;
    private float invincibleTimer;
    const float INVINCIBLE_TIME = 0.2f;

    int debugHealth = 100;

    public override void OnNetworkSpawn()
    {
        _health.Value = _maxHealth;
        if (side == Side.Player) ownerId = OwnerClientId;
    }

    private void Update()
    {
        if (isInvincible) invincibleTimer -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        debugHealth -= damage;
        if (!IsServer) return;

        if (isInvincible) return;
        _health.Value -= damage;
        _health.Value = Mathf.Max(0, _health.Value);
        invincibleTimer = INVINCIBLE_TIME;
        onHurt.Invoke();
        if (_health.Value <= 0)
        {
            if (deathParticles != null)
            {
                deathParticles.SetActive(true);
                deathParticles.transform.SetParent(null);
            }
            onDeath.Invoke();
            return;
        }
        shakeTween?.Kill();
        origPos = transform.position;
        shakeTween = transform.DOShakePosition(0.3f, 0.2f, 30).OnComplete(() => transform.position = origPos);
    }

    public override void OnNetworkDespawn()
    {
        shakeTween?.Kill();
    }

    // private void OnGUI()
    // {
    //     if (!IsOwner)
    //     {
    //         GUILayout.BeginArea(new Rect(0, 300, 200, 200));
    //         GUILayout.Label($"Other Health: {health}");
    //         GUILayout.EndArea();
    //     }
    //     else
    //     {
    //         GUILayout.Label($"Self Health: {health}");
    //         GUILayout.Label($"Debug Health: {debugHealth}");
    //     }
    // }
}
