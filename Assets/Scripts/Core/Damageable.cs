using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public Side side => _side;
    public int maxHealth => _maxHealth;
    public int health => _health;
    public UnityEvent OnHurt;
    public UnityEvent OnDeath;
    [SerializeField] int _maxHealth;
    [SerializeField] Side _side;
    [SerializeField] GameObject deathParticles;
    int _health;
    private Vector3 origPos;
    private Tween shakeTween;
    private bool isInvincible => invincibleTimer > 0;
    private float invincibleTimer;
    const float INVINCIBLE_TIME = 0.2f;

    private void Awake()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        if (isInvincible) invincibleTimer -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        _health -= damage;
        _health = Mathf.Max(0, _health);
        invincibleTimer = INVINCIBLE_TIME;
        OnHurt.Invoke();
        if (_health <= 0)
        {
            if (deathParticles != null)
            {
                deathParticles.SetActive(true);
                deathParticles.transform.SetParent(null);
            }
            OnDeath.Invoke();
            Destroy(gameObject);
            return;
        }
        shakeTween?.Kill();
        origPos = transform.position;
        shakeTween = transform.DOShakePosition(0.3f, 0.2f, 30).OnComplete(() => transform.position = origPos);
    }

    private void OnDestroy()
    {
        shakeTween?.Kill();
    }
}
