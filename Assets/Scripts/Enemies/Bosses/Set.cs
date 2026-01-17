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
    [SerializeField] private List<GameObject> hangingProjectileSprites;
    [SerializeField] private float specialFireRate = .5f;
    private float _hangingFireCounter = 0f;
    
    private int _hangingProjectileCounter = 0;

    
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
    


    private float _attackDurationCounter = 0f;

    private bool _phase2;

    private bool _rollNewAttack = true;
    private SetAttacks _currentAttack;
    private float _attackDelayCounter = 0;
    
    
    protected override void Start()
    {
        base.Start();
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
        if (_hangingFireCounter <= specialFireRate)
        {
            hangingProjectileSprites[_hangingProjectileCounter%8].SetActive(true);
            _hangingFireCounter += Time.fixedDeltaTime;
        }
        else
        {
            SpawnHangingProjectile();
            _hangingFireCounter = 0;
        }
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
        if (_attackDurationCounter <= delayedShootDuration)
        {
            CrossMove();
            if (_hangingFireCounter <= specialFireRate)
            {
                hangingProjectileSprites[_hangingProjectileCounter%8].SetActive(true);
                _hangingFireCounter += Time.fixedDeltaTime;
            }
            else
            {
                SpawnHangingProjectile();
                _hangingFireCounter = 0;
            }

            _attackDurationCounter += Time.fixedDeltaTime;
        }
        else
        {
            ClearProjectileSprites();
            DelayNextAttack();
        }
    }

    private void SpawnHangingProjectile()
    {
        Vector2 spawnPos = hangingProjectileSprites[_hangingProjectileCounter % 8].transform.position;
        Debug.Log(spawnPos);
        hangingProjectileSprites[_hangingProjectileCounter % 8].SetActive(false);
        projectileFactory.ShootFromPosition((_player.transform.position - (Vector3)spawnPos).normalized,spawnPos);

        _hangingProjectileCounter++;

    }

    private void ClearProjectileSprites()
    {
        foreach (GameObject spriteObject in hangingProjectileSprites)
        {
            spriteObject.SetActive(false);
        }
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

        if (val >= 70)//70
        {
            _animator.SetBool("isWalking",true);
            return SetAttacks.ShootWalk;
        }

        if (val >= 35)//45
        {
            _dashing = true;
            _startDash = true;
            _windUpCounter = 0f;
            _animator.SetBool("isWalking",true);
            return SetAttacks.Dash;
        }

        if (val >= 0)//20
        {
            _hangingProjectileCounter = 0;
            _agent.enabled = false;
            _animator.SetBool("isWalking",true);
            return SetAttacks.DelayedShoot;
        }

        _plumming = true;
        _startPlum = true;
        return SetAttacks.BabyPlum;
    }

    private SetAttacks RollAttackPhase2()
    {
        int val = Random.Range(0, 100);

        if (val >= 50)//65
        {
            _dashing = true;
            _startDash = true;
            _windUpCounter = 0f;
            _animator.SetBool("isWalking",true);
            return SetAttacks.Dash;
        }

        if (val >= 0)//25
        {
            _animator.SetBool("isWalking",true);
            return SetAttacks.ShootWalk;
        }

        _plumming = true;
        _startPlum = true;
        return SetAttacks.BabyPlum;
    }

    private void DelayNextAttack()
    {
        _animator.SetBool("isWalking",false);
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
        {
            PushBackReflect(other,1f);
            FinishDash();
        }
            
    }

    private void Bounce(Collision2D other)
    {
        Vector2 normal = other.GetContact(0).normal;

        Vector2 currentVelocity = _rb.velocity;

        Vector2 reflectedDir = Vector2.Reflect(currentVelocity.normalized, normal);

        float speed = currentVelocity.magnitude;


        if (speed < EnemyStats.GetMovementSpeed())
            speed = EnemyStats.GetMovementSpeed();

        _rb.velocity = reflectedDir * speed;

        _rb.position += normal * 0.05f;

        _plumDir = reflectedDir;
    }

    protected override void OnHpChangedHandler(int hp, int maxHp)
    {
        base.OnHpChangedHandler(hp, maxHp);

        if (hp <= (maxHp / 10) * 4)
            _phase2 = true;
    }
    
    protected override IEnumerator ExitCoroutine()
    {
        _animator.SetBool("isWalking",true);
        tag = "Untagged";
        ClearProjectileSprites();
        _agent.enabled = true;
            
        while (Vector2.Distance(transform.position, exit.transform.position) > 0.1f)
        {
            NavMoveTo(exit.transform);
            yield return null;
        }

        _animator.SetBool("isWalking",false);
        yield return new WaitForSeconds(1);

        heart.SetActive(true);
        _agent.enabled = false;
        Destroy(gameObject);
    }

}
