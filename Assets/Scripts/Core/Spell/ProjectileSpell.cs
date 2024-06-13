using UnityEngine;

public class ProjectileSpell : SpellBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float projectileSpeed;
    public override void Cast(PlayerLogic master)
    {
        var pos = master.transform.TransformPoint(spawnPoint.localPosition);
        var rot = master.transform.rotation * spawnPoint.localRotation;
        var fireball = Instantiate(projectilePrefab, pos, rot).GetComponent<ProjectileBase>();
        fireball.Init(3f);
    }
}
