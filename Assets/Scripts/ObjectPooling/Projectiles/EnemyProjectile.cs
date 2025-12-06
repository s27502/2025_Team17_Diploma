using UnityEngine;
using Player;

public class EnemyProjectile : ProjectileBase
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
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