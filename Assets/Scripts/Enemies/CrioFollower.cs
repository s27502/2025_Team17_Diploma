using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrioFollower : Enemy
{
    [SerializeField] private bool haltMovement = false;
    private float _shootTimer;
    private Vector2 _shootingDir;

    protected override void Attack()
    {
        if (!haltMovement)
        {
            CrossMove();
            _animator.SetBool("isWalking",true);
            _shootTimer += Time.fixedDeltaTime;

            if (!(_shootTimer >= EnemyStats.GetFireRate())) return;
            _shootTimer = 0f;
            Shoot();
        }
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
