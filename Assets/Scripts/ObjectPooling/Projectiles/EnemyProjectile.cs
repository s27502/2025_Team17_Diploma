using UnityEngine;
using Player;

public class EnemyProjectile : ProjectileBase
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyHitbox"))
            return;
        
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>()?.ModifyHp(-damage);
            _pool?.ReleaseObject(this);
            return;
        }
        
        _pool?.ReleaseObject(this);
    }
}