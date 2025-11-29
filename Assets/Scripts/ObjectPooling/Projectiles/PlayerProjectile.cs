using Enemies;
using Managers;
using UnityEngine;
using Player;

public class PlayerProjectile : ProjectileBase
{
    protected override void Awake()
    {
        base.Awake();
        damage = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats().GetDmg();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;


        if (other.CompareTag("Enemy"))
        {
            var enemyStats = other.GetComponent<EnemyStats>();
            enemyStats?.ModifyHp(-damage);
            _pool?.ReleaseObject(this);
            return;
        }
        
        _pool?.ReleaseObject(this);
    }
}