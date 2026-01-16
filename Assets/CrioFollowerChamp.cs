using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrioFollowerChamp : CrioFollower
{
    private bool shoot3 = true;

    protected override void Shoot()
    {
        if (_player)
        {
            _shootingDir = (_player.transform.position - transform.position).normalized;
        }

        if (shoot3)
        {
            Shoot3();
            shoot3 = false;
        }
        else
        {
            Shoot4();
            shoot3 = true;
        }
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
}
