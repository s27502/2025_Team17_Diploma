using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;

public enum RaAttacks
{
    Shoot,
    LaserCross,
    LaserX,
    Sun
}

public class Ra : Boss
{
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private float attackDelay = 5f;
    private float _attackDelayCounter = 3f;
    
    [SerializeField] private float sunAttackDuration = 10f;
    private float _sunDurationCounter = 0f;
    [SerializeField] private GameObject sun;
    [SerializeField] private float sunRotationSpeed;
    private bool _sunRotateClockwise;
    [SerializeField] private float sunRotationDelay = 1f;
    private float _rotationDelayCounter = 0f;

    [SerializeField] private float laserDuration = 2f;
    private float _laserDurationCounter = 0f;
    [SerializeField] private float laserCharge = 1f;
    private float _laserChargeCounter = 0f;

    [SerializeField] private GameObject LaserXObject;
    [SerializeField] private GameObject LaserCrossObject;

    [SerializeField] private float projectileDelay = 1f;
    private float _shootCounter = 0f;
    private float _projectileDelayCounter = 0f;

    private bool isAttacking;
    private bool isOnSun = false;

    private Vector2 _shootingDir;

    private RaAttacks _currentAttack;

    protected override void Attack()
    {
        if (!isAttacking)
        {
            CrossMove();
            _attackDelayCounter += Time.fixedDeltaTime;

            if (_attackDelayCounter >= attackDelay)
            {
                isAttacking = true;
                _currentAttack = RollAttack();
            }
        }
        else
        {
            Debug.Log(_currentAttack);
            switch (_currentAttack)
            {
                case RaAttacks.Shoot:
                    Shoot();
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
    
    private void Shoot()
    {
        _shootCounter -= Time.fixedDeltaTime;

        if (!(_shootCounter <= 0f)) return;
        Shoot4();
        isAttacking = false;
        _attackDelayCounter = 0;
    }

    private void Shoot3()
    {
        if (_player)
        {
            _shootingDir = (_player.transform.position - transform.position).normalized;
        }
        projectileFactory.Shoot(Rotate(_shootingDir,-40));
        projectileFactory.Shoot(_shootingDir);
        projectileFactory.Shoot(Rotate(_shootingDir,40));
    }
    
    private void Shoot4()
    {
        if (_player)
        {
            _shootingDir = (_player.transform.position - transform.position).normalized;
        }
        projectileFactory.Shoot(Rotate(_shootingDir,-20));
        projectileFactory.Shoot(Rotate(_shootingDir,20));
        projectileFactory.Shoot(Rotate(_shootingDir,-60));
        projectileFactory.Shoot(Rotate(_shootingDir,60));
    }
    
    private Vector2 Rotate(Vector2 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }
    
    

    private void LaserCross()
    {
        if (_laserChargeCounter >= 0)
        {
            _laserChargeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            LaserCrossObject.SetActive(true);
            if (_laserDurationCounter >= 0)
            {
                _laserDurationCounter -= Time.fixedDeltaTime;
            }
            else
            {
                LaserCrossObject.SetActive(false);
                isAttacking = false;
                _attackDelayCounter = 0;
            }
        }
    }

    private void LaserX()
    {
        if (_laserChargeCounter >= 0)
        {
            _laserChargeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            LaserXObject.SetActive(true);
            if (_laserDurationCounter >= 0)
            {
                _laserDurationCounter -= Time.fixedDeltaTime;
            }
            else
            {
                LaserXObject.SetActive(false);
                isAttacking = false;
                _attackDelayCounter = 0;
            }
        }
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
                transform.position = new Vector3(transform.position.x, transform.position.y - 25f, transform.position.z);


                Debug.Log(_sunDurationCounter);
            }
        }
        else
        {
            _sunDurationCounter -= Time.fixedDeltaTime;

            _rotationDelayCounter -= Time.fixedDeltaTime;

            if (_rotationDelayCounter <= 0)
            {
                sun.transform.Rotate(Vector3.forward, sunRotationSpeed * Time.fixedDeltaTime * (_sunRotateClockwise ? 1 : -1));
            }

            if (_sunDurationCounter <= 0f)
            {
                isOnSun = false;
                sun.SetActive(false);
                sun.transform.rotation = Quaternion.identity;
                isAttacking = false;
                _attackDelayCounter = 0;

                transform.position = sun.transform.position;
            }
        }
    }



    private bool GetSunDir()
    {
        return Random.Range(0, 2) == 0 ? true : false;
    }

    private RaAttacks RollAttack()
    {
        int val = Random.Range(0, 100);

        if (val >= 85)
        {
            _rotationDelayCounter = sunRotationDelay;
            _sunRotateClockwise = GetSunDir();
            _sunDurationCounter = sunAttackDuration;
            return RaAttacks.Sun;
        }

        if (val >= 50)
        {
            Shoot3();
            _shootCounter = EnemyStats.GetFireRate();
            return RaAttacks.Shoot;
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
    
    public override void FlipTo(float dirX)
    {
        float threshold = 0.01f;
        if (Mathf.Abs(dirX) < threshold) return;

        Vector3 scale = spriteObject.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -Mathf.Sign(dirX);
        spriteObject.transform.localScale = scale;
    }

    public override void Die()
    {
        sun.SetActive(false);
        LaserXObject.SetActive(false);
        LaserCrossObject.SetActive(false);
        base.Die();
    }
}
