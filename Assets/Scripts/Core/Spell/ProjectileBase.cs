using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private float speed;
    protected const float PROJECTILE_LIFETIME = 100f;

    public void Init(float speed)
    {
        this.speed = speed;
    }

    private void Awake()
    {
        Destroy(gameObject, PROJECTILE_LIFETIME);
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * transform.forward, Space.Self);
    }
}
