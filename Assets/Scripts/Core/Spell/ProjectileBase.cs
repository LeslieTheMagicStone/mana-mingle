using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private float speed;
    [SerializeField] private float lifeTime = 100f;

    public void Init(float speed)
    {
        this.speed = speed;
    }

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
