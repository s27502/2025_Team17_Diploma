using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AndroSphinxAttacks
{
    Charge,
    Shoot
}
public class AndroSphinx : Enemy
{
    [SerializeField] private float windUpTime = 2f;
    [SerializeField] private float _miniChargeCooldown = 0.5f;
    [SerializeField] private float _chargingSpeedMult = 5;
    
    private float _windingUpCounter;
    private bool _charging;
    private int _chargesNumber;
    private Vector2 _chargeDirection;
    private float _originalSpeed;
    private float _miniChargeCounter;
    
    private float _attackDurationCounter;
    private float _attackDuration;


    private float _shootPerpCounter = 0f;
    private float _shootCounter = 0f;


    private Vector2 _shootingDir;

    private bool nowShootRight = true;

    private AndroSphinxAttacks _currentAttack;
    
    protected override void Start()
    {
        base.Start();
        
        _windingUpCounter = windUpTime;
        _originalSpeed = EnemyStats.GetMovementSpeed();
        _currentAttack = AndroSphinxAttacks.Shoot;
    }

    protected override void Attack()
    {
        switch (_currentAttack)
        {
            case AndroSphinxAttacks.Charge:
                PerformChargeAttack();
                PerformShootPerpendicularly();
                break;
            case AndroSphinxAttacks.Shoot:
                PerformShootAttack();
                break;
        }
    }

    private void PerformShootAttack()
    {
        if (_attackDurationCounter <= EnemyStats.GetAtkSpd())
        {
            if (_shootCounter >= EnemyStats.GetFireRate())
            {
                switch (nowShootRight)
                {
                    case true:
                        if (_player)
                            _shootingDir = (_player.transform.position - transform.position).normalized;

                        projectileFactory.Shoot(RotateProjectile(_shootingDir, 15));
                        break;
                    case false:
                        if (_player)
                            _shootingDir = (_player.transform.position - transform.position).normalized;

                        projectileFactory.Shoot(RotateProjectile(_shootingDir, -15));
                        break;
                }

                nowShootRight = !nowShootRight;
                _shootCounter = 0;
            }

            _shootCounter += Time.fixedDeltaTime;
        }
        else
        {
            _currentAttack = AndroSphinxAttacks.Charge;
        }

        _attackDurationCounter += Time.fixedDeltaTime;

    }

    private void PerformShootPerpendicularly()
    {
        if (_shootPerpCounter >= EnemyStats.GetFireRate())
        {
            ShootPerpendicularly();
            _shootPerpCounter = 0;
        }

        _shootPerpCounter += Time.fixedDeltaTime;
    }

    private void ShootPerpendicularly()
    {

        Vector2 dir1 = new Vector2(-_chargeDirection.y, _chargeDirection.x);
        Vector2 dir2 = new Vector2(_chargeDirection.y, -_chargeDirection.x);
        
        projectileFactory.Shoot(dir1);
        projectileFactory.Shoot(dir2);
    }
    
    private void PerformChargeAttack()
    {
        if (!_charging)
        {
            _windingUpCounter -= Time.fixedDeltaTime;
            if (_windingUpCounter <= 0)
                StartCharge();
            return;
        }

        if (_miniChargeCounter > 0)
        {
            _miniChargeCounter -= Time.fixedDeltaTime;
            return;
        }

        if (_chargesNumber > 0)
            MoveInDirection(_chargeDirection);

        if (_chargesNumber <= 0)
        {
            _charging = false;
            EnemyStats.SetMovementSpeed(_originalSpeed);
            _windingUpCounter = windUpTime;
            _attackDurationCounter = 0;
            _currentAttack = AndroSphinxAttacks.Shoot;
        }
    }
    
    private void StartCharge()
    {
        _charging = true;
        EnemyStats.SetMovementSpeed(_originalSpeed * _chargingSpeedMult);
        _chargesNumber = Random.Range(1, 3);
        _chargeDirection = (_player.transform.position - transform.position).normalized;
        FlipTo(_chargeDirection.x);
        _miniChargeCounter = 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((!other.gameObject.CompareTag("Obstacle") && !other.gameObject.CompareTag("Player")) || !_charging)
            return;

        _chargesNumber--;
        _chargeDirection = (_player.transform.position - transform.position).normalized;
        _miniChargeCounter = _miniChargeCooldown;
    }
}
