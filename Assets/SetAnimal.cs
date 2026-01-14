using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimal : Enemy
{
    private float _shootCounter;
    private bool nowShoot4 = false;
    private Vector2 _shootingDir;
    
    protected override void Attack()
    {
        NavMoveTo(_player.transform);
        PerformShooting();
    }

    private void PerformShooting()
    {
        if (_shootCounter >= EnemyStats.GetFireRate())
        {
            switch (nowShoot4)
            {
                case true:
                    Shoot4();
                    break;
                case false:
                    Shoot3();
                    break;
            }

            nowShoot4 = !nowShoot4;
            _shootCounter = 0;
        }

        _shootCounter += Time.fixedDeltaTime;
    }
    
    private void Shoot4()
    {
        if (_player)
            _shootingDir = (_player.transform.position - transform.position).normalized;

        projectileFactory.Shoot(RotateProjectile(_shootingDir, -20));
        projectileFactory.Shoot(RotateProjectile(_shootingDir, 20));
        projectileFactory.Shoot(RotateProjectile(_shootingDir, -60));
        projectileFactory.Shoot(RotateProjectile(_shootingDir, 60));
    }
    
    private void Shoot3()
    {
        if (_player)
        {
            _shootingDir = (_player.transform.position - transform.position).normalized;
        }
        projectileFactory.Shoot(RotateProjectile(_shootingDir,-40));
        projectileFactory.Shoot(_shootingDir);
        projectileFactory.Shoot(RotateProjectile(_shootingDir,40));
    }
    


    public override void Die()
    {
        ShootCross();
        base.Die();
    }
    
    private void ShootCross()
    {
        Vector2[] dirs =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        foreach (var dir in dirs)
            projectileFactory.Shoot(dir);
    }
}
