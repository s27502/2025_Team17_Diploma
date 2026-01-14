using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public enum RaAttacks
{
    Spiral,
    LaserCross,
    LaserX,
    Sun
}

public class Ra : Boss
{
    [SerializeField] private float attackDelay = 5f;
    private float _shootCounter;
    private Vector2 _shootingDir;
    private float _attackDelayCounter = 3f;

    [Header("Spiral Attack")]
    [SerializeField] private float _spiralFireRate = 0.2f;
    [SerializeField] private float _spiralDuration = 6f;
    private float _spiralAngle;
    private float _spiralShootCounter;
    private float _spiralDurationCounter;

    [Header("Sun Attack")]
    [SerializeField] private float sunAttackDuration = 10f;
    private float _sunDurationCounter;
    [SerializeField] private GameObject sun;
    [SerializeField] private float sunRotationSpeed;
    private bool _sunRotateClockwise;
    [SerializeField] private float sunRotationDelay = 1f;
    private float _rotationDelayCounter;

    [Header("Lasers")]
    [SerializeField] private float laserDuration = 2f;
    [SerializeField] private float laserCharge = 1f;
    private float _laserDurationCounter;
    private float _laserChargeCounter;
    [SerializeField] private GameObject LaserXObject;
    [SerializeField] private GameObject LaserCrossObject;

    private bool isAttacking;
    private bool isOnSun;

    private RaAttacks _currentAttack;

    protected override void Start()
    {
        base.Start();
        _agent.enabled = false;
    }

    protected override void Attack()
    {
        if (!isAttacking)
        {
            CrossMove();
            Shoot4();
            _attackDelayCounter += Time.fixedDeltaTime;

            if (_attackDelayCounter >= attackDelay)
            {
                isAttacking = true;
                _currentAttack = RollAttack();
            }
        }
        else
        {
            switch (_currentAttack)
            {
                case RaAttacks.Spiral:
                    SpiralAttack();
                    break;
                case RaAttacks.LaserCross:
                    LaserCross();
                    break;
                case RaAttacks.LaserX:
                    LaserX();
                    break;
                case RaAttacks.Sun:
                    SunAttack();
                    break;
            }
        }
    }
    
    private void Shoot4()
    {
        if (_shootCounter <= 0)
        {
            if (_player)
                _shootingDir = (_player.transform.position - transform.position).normalized;

            projectileFactory.Shoot(Rotate(_shootingDir, -20));
            projectileFactory.Shoot(Rotate(_shootingDir, 20));
            projectileFactory.Shoot(Rotate(_shootingDir, -60));
            projectileFactory.Shoot(Rotate(_shootingDir, 60));

            _shootCounter = EnemyStats.GetFireRate();
        }

        _shootCounter -= Time.deltaTime;

    }
    
    private void SpiralAttack()
    {
        if (_spiralDurationCounter <= 0)
        {
            isAttacking = false;
            _attackDelayCounter = 0;
            return;
        }

        if (_spiralShootCounter <= 0)
        {
            _spiralShootCounter = _spiralFireRate;

            Vector2[] dirs =
            {
                Vector2.up,
                Vector2.down,
                Vector2.left,
                Vector2.right
            };

            foreach (var dir in dirs)
            {
                projectileFactory.Shoot(Rotate(dir, _spiralAngle));
            }

            _spiralAngle += 10f;
        }

        _spiralShootCounter -= Time.fixedDeltaTime;
        _spiralDurationCounter -= Time.fixedDeltaTime;
    }

    private Vector2 Rotate(Vector2 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }
    
    private void LaserCross()
    {
        if (_laserChargeCounter > 0)
        {
            _laserChargeCounter -= Time.fixedDeltaTime;
            return;
        }

        LaserCrossObject.SetActive(true);

        if (_laserDurationCounter > 0)
        {
            _laserDurationCounter -= Time.fixedDeltaTime;
            return;
        }

        LaserCrossObject.SetActive(false);
        isAttacking = false;
        _attackDelayCounter = 0;
    }

    private void LaserX()
    {
        if (_laserChargeCounter > 0)
        {
            _laserChargeCounter -= Time.fixedDeltaTime;
            return;
        }

        LaserXObject.SetActive(true);

        if (_laserDurationCounter > 0)
        {
            _laserDurationCounter -= Time.fixedDeltaTime;
            return;
        }

        LaserXObject.SetActive(false);
        isAttacking = false;
        _attackDelayCounter = 0;
    }
    
    private void SunAttack()
    {
        float distance = Vector3.Distance(transform.position, sun.transform.position);

        if (!isOnSun)
        {
            if (distance > 0.1f)
            {
                MoveTo(sun.transform.position);
            }
            else
            {
                isOnSun = true;
                sun.SetActive(true);
                transform.position += Vector3.down * 25f;
            }
            return;
        }

        _sunDurationCounter -= Time.fixedDeltaTime;
        _rotationDelayCounter -= Time.fixedDeltaTime;

        if (_rotationDelayCounter <= 0)
        {
            sun.transform.Rotate(
                Vector3.forward,
                sunRotationSpeed * Time.fixedDeltaTime * (_sunRotateClockwise ? 1 : -1)
            );
        }

        if (_sunDurationCounter <= 0)
        {
            isOnSun = false;
            sun.SetActive(false);
            sun.transform.rotation = Quaternion.identity;
            transform.position = sun.transform.position;
            isAttacking = false;
            _attackDelayCounter = 0;
        }
    }

    private bool GetSunDir() => Random.Range(0, 2) == 0;

    private RaAttacks RollAttack()
    {
        int val = Random.Range(0, 100);

        if (val >= 85)
        {
            _sunDurationCounter = sunAttackDuration;
            _rotationDelayCounter = sunRotationDelay;
            _sunRotateClockwise = GetSunDir();
            return RaAttacks.Sun;
        }

        if (val >= 50)
        {
            _spiralDurationCounter = _spiralDuration;
            _spiralAngle = 0;
            _spiralShootCounter = 0;
            return RaAttacks.Spiral;
        }

        if (val >= 25)
        {
            _laserDurationCounter = laserDuration;
            _laserChargeCounter = laserCharge;
            return RaAttacks.LaserCross;
        }

        _laserDurationCounter = laserDuration;
        _laserChargeCounter = laserCharge;
        return RaAttacks.LaserX;
    }

    public override void Die()
    {
        sun.SetActive(false);
        LaserXObject.SetActive(false);
        LaserCrossObject.SetActive(false);
        base.Die();
    }
}
