using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private GameObject hitParticles;
    public void OnHurt()
    {
        if (hitParticles != null)
        {
            hitParticles.SetActive(true);
            var ps = hitParticles.GetComponent<ParticleSystem>();
            if (ps != null)            {
                ps.Play();
            }
        }
    }

   public void OnDeath()
    {
        gameObject.SetActive(false);
    }
}
