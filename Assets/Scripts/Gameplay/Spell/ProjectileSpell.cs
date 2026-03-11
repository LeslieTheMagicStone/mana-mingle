using Unity.Netcode;
using UnityEngine;

public class ProjectileSpell : SpellBase
{
    [SerializeField] public Damager projectilePrefab;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public float projectileSpeed;
    
}
