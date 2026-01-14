using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;


public enum SetAttacks
{
    ShootWalk,
    Dash,
    DelayedShoot,
    BabyPlum
}
public class Set : Boss
{
    private List<Vector2> _delayedShootPositions;
    private Vector2 _delayedShootPos;
    private Vector2 _delayedShootDir;
    
    [SerializeField] private float shootWalkDuration = 5f;
    private float _shootWalkFireCounter = 0f;
    private Vector2 _shootWalkDir;
    private bool _nowShoot4 = false;
    
    [SerializeField] private float delayedShootDuration = 4f;
    [SerializeField] private float babyPlumDuration = 8f;
    
    [SerializeField] private float delayedProjectileOffset = 1f;
    [SerializeField] private float specialFireRate = .5f;

    private float _attackDurationCounter = 0f;

    private bool _phase2;

    private bool _rollNewAttack = true;
    private SetAttacks _currentAttack;
    
    private static readonly Vector2[] _defaultShootPositions =
    {
        Vector2.up,
        new Vector2(-1, 1).normalized,
        Vector2.left,
        new Vector2(-1, -1).normalized,
        Vector2.down,
        new Vector2(1, -1).normalized,
        Vector2.right,
        new Vector2(1, 1).normalized
    };
    
    protected override void Start()
    {
        base.Start();
        _delayedShootPositions = new List<Vector2>();
    }

    protected override void Attack()
    {
        if (_rollNewAttack)
        {
            switch (_phase2)
            {
                case false:
                    _currentAttack = RollAttack();
                    _rollNewAttack = false;
                    break;
                case true:
                    _currentAttack = RollAttackPhase2();
                    _rollNewAttack = false;
                    break;
            }
        }
        else
        {
            switch (_currentAttack)
            {
                case SetAttacks.ShootWalk:
                    PerformShootWalk();
                    break;
                case SetAttacks.DelayedShoot:
                    PerformDelayedShoot();
                    break;
                case SetAttacks.Dash:
                    PerformDash();
                    break;
                case SetAttacks.BabyPlum:
                    PerformBabyPlum();
                    break;
            }

            if (_phase2)
            {
                PerformDelayedShoot2();
            }
        }
    }

    private void PerformDelayedShoot2()
    {
        throw new System.NotImplementedException();
    }

    private void PerformBabyPlum()
    {
        throw new System.NotImplementedException();
    }

    private void PerformDash()
    {
        throw new System.NotImplementedException();
    }

    private void PerformDelayedShoot()
    {
        throw new System.NotImplementedException();
    }

    private void PerformShootWalk()
    {
        NavMoveTo(_player.transform);
        PerformWalkShooting();
    }
    
    private void PerformWalkShooting()
    {
        if (_shootWalkFireCounter >= EnemyStats.GetFireRate())
        {
            switch (_nowShoot4)
            {
                case true:
                    Shoot4();
                    break;
                case false:
                    Shoot3();
                    break;
            }

            _nowShoot4 = !_nowShoot4;
            _shootWalkFireCounter = 0;
        }

        _shootWalkFireCounter += Time.fixedDeltaTime;
    }
    
    private void Shoot4()
    {
        if (_player)
            _shootWalkDir = (_player.transform.position - transform.position).normalized;

        projectileFactory.Shoot(RotateProjectile(_shootWalkDir, -20));
        projectileFactory.Shoot(RotateProjectile(_shootWalkDir, 20));
        projectileFactory.Shoot(RotateProjectile(_shootWalkDir, -60));
        projectileFactory.Shoot(RotateProjectile(_shootWalkDir, 60));
    }
    
    private void Shoot3()
    {
        if (_player)
        {
            _shootWalkDir = (_player.transform.position - transform.position).normalized;
        }
        projectileFactory.Shoot(RotateProjectile(_shootWalkDir,-40));
        projectileFactory.Shoot(_shootWalkDir);
        projectileFactory.Shoot(RotateProjectile(_shootWalkDir,40));
    }

    private SetAttacks RollAttack()
    {
        
        int val = Random.Range(0, 100);

        if (val >= 0)
        {
            return SetAttacks.ShootWalk;
        }

        if (val >= 45)
        {
            return SetAttacks.Dash;
        }

        if (val >= 20)
        {
            return SetAttacks.DelayedShoot;
        }

        return SetAttacks.BabyPlum;
    }

    private SetAttacks RollAttackPhase2()
    {
        int val = Random.Range(0, 100);

        if (val >= 65)
        {
            return SetAttacks.Dash;
        }

        if (val >= 25)
        {
            return SetAttacks.ShootWalk;
        }

        return SetAttacks.BabyPlum;
    }
    
    private void UpdateProjectileSpawnpoints()
    {
        _delayedShootPositions.Clear();

        Vector2 origin = transform.position;

        foreach (Vector2 dir in _defaultShootPositions)
        {
            _delayedShootPositions.Add(origin + dir * delayedProjectileOffset);
        }
    }
}
