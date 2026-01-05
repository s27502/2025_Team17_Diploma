using UnityEngine;
using Player;
using StatusSystem;

namespace ObjectPooling.Projectiles
{
    public class EnemyPoisonProjectile : ProjectileBase
    {
        [SerializeField] private float _poisonDuration = 5;
        [SerializeField] private float _poisonDmg = 1;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
                return;
        
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerStats>()?.ModifyHp(-damage);
                other.GetComponent<StatusHandler>().Poison(_poisonDuration,_poisonDmg);
                _pool?.ReleaseObject(this);
                return;
            }
        
            _pool?.ReleaseObject(this);
        }
    }
}