using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


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
    
    private bool _dashing;
    private bool _startDash;
    [SerializeField] private float dashWindUp;
    private float _windUpCounter = 0f;
    private Vector2 _dashDir;
    private float _originalSpeed;
    [SerializeField] private float _dashSpeedMult = 2f;
    
    [SerializeField] private float delayedShootDuration = 4f;
    
    [SerializeField] private float babyPlumDuration = 8f;
    [SerializeField] private float plumSpeedMult = 2f;
    private Vector2 _plumDir;
    private bool _plumming = false;
    private bool _startPlum = false;
    
    [SerializeField] private float delayedProjectileOffset = 1f;
    [SerializeField] private float specialFireRate = .5f;

    private float _attackDurationCounter = 0f;

    private bool _phase2;

    private bool _rollNewAttack = true;
    private SetAttacks _currentAttack;
    private float _attackDelayCounter = 0;
    
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
        _originalSpeed = EnemyStats.GetMovementSpeed();
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
        if (_plumming)
        {
            if (_startPlum)
            {
                _agent.enabled = false;
                _startPlum = false;
                
                _plumDir = PickNewRandomDir360();
                _rb.velocity = _plumDir * EnemyStats.GetMovementSpeed();
            }

            if (_attackDurationCounter <= babyPlumDuration)
            {
                _attackDurationCounter += Time.fixedDeltaTime;

                //shoot
            }
            else
            {
                _plumming = false;
                _rb.velocity = Vector2.zero;
            }

        }
        else
        {
            DelayNextAttack();
        }
    }
    
    private Vector2 PickNewRandomDir360()
    {
        float angle = Random.Range(0f, 360f);
        Debug.Log(angle);
        return new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ).normalized;
    }
    
    public Vector3 GetRandomDirection()
    {

        float randomAngle = Random.Range(0f, 360f);
        
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);

        Vector3 newDirection = rotation * transform.forward;

        return newDirection.normalized;
    }


    private void PerformDash()
    {
        if (_dashing)
        {
            if (_startDash)
            {
                StartDash();
            }
            else
            {
                MoveInDirection(_dashDir);
            }
        }
        else
        {
            DelayNextAttack();
        }
    }

    private void StartDash()
    {
        if (_windUpCounter <= dashWindUp)
        {
            _windUpCounter += Time.fixedDeltaTime;
        }
        else
        {
            _agent.enabled = false;
            EnemyStats.SetMovementSpeed(_originalSpeed * _dashSpeedMult);
            _dashDir = (_player.transform.position - transform.position).normalized;

            _startDash = false;
        }
    }

    private void FinishDash()
    {
        EnemyStats.SetMovementSpeed(_originalSpeed);
        _dashing = false;
        //_agent.enabled = true;
    }

    private void PerformDelayedShoot()
    {
        throw new System.NotImplementedException();
    }

    private void PerformShootWalk()
    {
        if (_attackDurationCounter <= shootWalkDuration)
        {
            NavMoveTo(_player.transform);
            PerformWalkShooting();
            _attackDurationCounter += Time.fixedDeltaTime;
        }
        else
        {
            DelayNextAttack();
        }
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
        _attackDurationCounter = 0f;
        int val = Random.Range(0, 100);

        if (val >= 101)//70
        {
            return SetAttacks.ShootWalk;
        }

        if (val >= 101)//45
        {
            _dashing = true;
            _startDash = true;
            _windUpCounter = 0f;
            return SetAttacks.Dash;
        }

        if (val >= 101)//20
        {
            return SetAttacks.DelayedShoot;
        }

        _plumming = true;
        _startPlum = true;
        return SetAttacks.BabyPlum;
    }

    private SetAttacks RollAttackPhase2()
    {
        int val = Random.Range(0, 100);

        if (val >= 65)
        {
            _dashing = true;
            _startDash = true;
            _windUpCounter = 0f;
            return SetAttacks.Dash;
        }

        if (val >= 25)
        {
            return SetAttacks.ShootWalk;
        }

        _plumming = true;
        
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
            _agent.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_plumming && (other.collider.CompareTag("Obstacle") || other.collider.CompareTag("Player")))
        {
            Bounce(other);
        }

        if (_dashing)
            FinishDash();
    }

    private void Bounce(Collision2D other)
    {
        _rb.velocity = -_rb.velocity;
    }



}
