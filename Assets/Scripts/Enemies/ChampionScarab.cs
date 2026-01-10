using System;
using UnityEngine;

namespace Enemies
{
    public class ChampionScarab : Scarab
    {
        private float _shootTimer;
        private Vector2 _shootingDir;
        protected override void Attack()
        {
            base.Attack();
            
            _shootTimer += Time.fixedDeltaTime;

            if (!(_shootTimer >= EnemyStats.GetFireRate())) return;
            _shootTimer = 0f;
            Shoot();
        }

        private void Shoot()
        {
            if (_player)
            {
                _shootingDir = (_player.transform.position - transform.position).normalized;
            }
            projectileFactory.Shoot(Rotate(_shootingDir,-40));
            projectileFactory.Shoot(_shootingDir);
            projectileFactory.Shoot(Rotate(_shootingDir,40));
            
            
        }
        
        private Vector2 Rotate(Vector2 v, float angle)
        {
            return Quaternion.Euler(0, 0, angle) * v;
        }
    }
}