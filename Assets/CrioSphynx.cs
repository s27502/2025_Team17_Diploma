using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrioSphynxAttacks
{
    WaveShoot,
    Shoot345
}

public class CrioSphynx : Enemy
{
    private float _attackDelayCounter = 0f;
    private bool _rollNewAttack = true;
    private CrioSphynxAttacks _currentAttack;
    private int _currentShotNum;
    private Vector2 _shootingDir;
    private List<Vector2> _shootingDirections = new List<Vector2>();

    private float _shootCounter = 0f;
    private bool _delay;
    
    protected override void Attack()
    {
        if (_rollNewAttack)
        {
            _currentAttack = RollAttack();
        }
        else
        {
            switch (_currentAttack)
            {
                case CrioSphynxAttacks.WaveShoot:
                    PerformWaveShoot();
                    break;
                case CrioSphynxAttacks.Shoot345:
                    PerformShoot();
                    break;
            }
        }
    }

    private void PerformWaveShoot()
    {
        if (_currentShotNum < 6)
        {
            if (_delay && _shootCounter <= EnemyStats.GetFireRate()*2)
            {
                _shootCounter += Time.fixedDeltaTime;
            }
            else if (_shootCounter <= EnemyStats.GetFireRate())
            {
                _shootCounter += Time.fixedDeltaTime;
            }
            else
            {
                switch (_currentShotNum)
                {
                    case 0:
                        _shootingDir = (_player.transform.position - transform.position).normalized;
                        _shootingDirections.Add(RotateProjectile(_shootingDir,-40));
                        _shootingDirections.Add(_shootingDir);
                        _shootingDirections.Add(RotateProjectile(_shootingDir,40));
                        break;
                    case 1:
                        break;
                    case 2:
                        _delay = true;
                        break;
                    case 3:
                        _delay = false;
                        _shootingDir = (_player.transform.position - transform.position).normalized;
                        _shootingDirections.Add(RotateProjectile(_shootingDir,40));
                        _shootingDirections.Add(_shootingDir);
                        _shootingDirections.Add(RotateProjectile(_shootingDir,-40));
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                }
                
                projectileFactory.Shoot(_shootingDirections[_currentShotNum]);
                _currentShotNum += 1;

                _shootCounter = 0f;
            }
        }
        else
        {
            DelayNextAttack();
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
    
    private void Shoot5()
    {
        if (_player)
        {
            _shootingDir = (_player.transform.position - transform.position).normalized;
        }
        projectileFactory.Shoot(RotateProjectile(_shootingDir,-60));
        projectileFactory.Shoot(RotateProjectile(_shootingDir,-30));
        projectileFactory.Shoot(_shootingDir);
        projectileFactory.Shoot(RotateProjectile(_shootingDir,30));
        projectileFactory.Shoot(RotateProjectile(_shootingDir,60));
    }

    private void PerformShoot()
    {
        if (_currentShotNum < 3)
        {
            if (_shootCounter <= EnemyStats.GetFireRate())
            {
                _shootCounter += Time.fixedDeltaTime;
            }
            else
            {
                switch (_currentShotNum)
                {
                    case 0:
                        Shoot3();
                        break;
                    case 1:
                        Shoot4();
                        break;
                    case 2:
                        Shoot5();
                        break;
                }

                _currentShotNum += 1;
                _shootCounter = 0f;
            }
        }
        else
        {
            DelayNextAttack();
        }
    }

    private CrioSphynxAttacks RollAttack()
    {
        _currentShotNum = 0;
        _shootCounter = 0f;
        _rollNewAttack = false;

        int val = Random.Range(0, 2);
        if (val == 0)
        {
            _shootingDirections.Clear();
            _delay = false;
            return CrioSphynxAttacks.WaveShoot;
        }
        
        return CrioSphynxAttacks.Shoot345;
    }
    
    private void DelayNextAttack()
    {
        if (_attackDelayCounter <= EnemyStats.GetAtkSpd())
        {
            _attackDelayCounter += Time.fixedDeltaTime;
        }
        else
        {
            _rollNewAttack = true;
            _attackDelayCounter = 0f;
        }
    }
}
