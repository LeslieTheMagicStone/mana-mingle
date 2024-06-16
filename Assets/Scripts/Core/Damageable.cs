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
    public bool isDead => health <= 0;
    [SerializeField] private Side _side;
    [SerializeField] private int _maxHealth;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private bool shake;
    public NetworkVariable<int> _health = new(writePerm: NetworkVariableWritePermission.Server);
    private Vector3 origPos;
    private Tween shakeTween;
    private bool isInvincible => invincibleTimer > 0;
    private float invincibleTimer;
    const float INVINCIBLE_TIME = 0.2f;

    private int dustormCount = 0;
    private bool isInDustStorm => dustormCount > 0;

    public override void OnNetworkSpawn()
    {
        _health.Value = _maxHealth;
        if (side == Side.Player) ownerId = OwnerClientId;
    }

    private void Update()
    {
        if (isInvincible) invincibleTimer -= Time.deltaTime;

        if (isInDustStorm) TakeDamage(1);
    }

    private void FixedUpdate()
    {
        dustormCount = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsServer) return;
        if (other.CompareTag("DustStorm"))
        {
            dustormCount++;
        }
    }

    public void TakeDamage(int damage)
    {
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
            OnDeathClientRpc();
            return;
        }
        if (shake)
        {
            shakeTween?.Kill();
            origPos = transform.position;
            shakeTween = transform.DOShakePosition(0.3f, 0.2f, 30).OnComplete(() => transform.position = origPos);
        }
    }

    [ClientRpc]
    private void OnDeathClientRpc()
    {
        onDeath.Invoke();
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
