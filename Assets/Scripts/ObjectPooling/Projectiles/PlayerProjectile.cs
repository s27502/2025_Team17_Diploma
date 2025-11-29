using UnityEngine;
using Player;

public class PlayerProjectile : ProjectileBase
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;


        if (other.CompareTag("Enemy") || other.CompareTag("EnemyHitbox"))
        {
            //var enemyStats = other.GetComponent<EnemyStats>();
            //enemyStats?.ModifyHp(-damage);
            _pool?.ReleaseObject(this);
            return;
        }
        
        _pool?.ReleaseObject(this);
    }
}