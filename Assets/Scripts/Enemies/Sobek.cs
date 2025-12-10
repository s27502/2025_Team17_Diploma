using System.Collections.Generic;
using System.IO.IsolatedStorage;
using DefaultNamespace.Factory;
using UnityEngine;

public enum SobekAttacks
{
    Charge,
    Spiral,
    RandomMoving
}

namespace Enemies
{
    public class Sobek : Boss
    {
        [SerializeField] private GameObject _enemies;
        [SerializeField] private float _windUpTime = 2f;
        [SerializeField] private float _miniChargeCooldown = 0.5f;
        [SerializeField] private float _chargingSpeedMult = 5;

        [SerializeField] private float _summonDelay = 10f;
        private float _summonDelayCounter = 0f;

        private float _spiralAngle = 0;
        [SerializeField] private float _spiralFireRate = .2f;
        
        [SerializeField] private GameObject _rainSpawner;
        [SerializeField] private GameObject _enemySpawnerObject;
        private EnemySpawner _spawner;
        private float _shootCounter;

        private SobekAttacks _currentAttack;

        private float _attackDurationCounter;
        private float _attackDuration;

        private float _windingUpCounter;
        private bool _charging = false;
        private int _chargesNumber = 0;
        private Vector2 _chargeDirection;
        private float _originalSpeed;
        private float _miniChargeCounter = 0f;
        
        private Vector2 _currentRandomTarget;
        [SerializeField] private float _posChangeInterval = 0.5f;
        private float _posChangeCounter = 0f;
        [SerializeField] private float _randomMoveDistance = 2f;

        [SerializeField] private float _spiralDuration = 7f;
        [SerializeField] private float _movingDuration = 5f;

        private float _attackDelayCounter;
        [SerializeField] private int _maxGators = 2;


        protected override void Start()
        {
            base.Start();
            _spawner = _enemySpawnerObject.GetComponent<EnemySpawner>();

            _attackDurationCounter = 0;
            _attackDelayCounter = 0;
            _summonDelayCounter = 15;
            
            _spawner.SpawnAtRandomPosition();
            _spawner.SpawnAtRandomPosition();
            
            _windingUpCounter = _windUpTime;
            _originalSpeed = EnemyStats.GetMovementSpeed();
        }
        
        

        protected override void Attack()
        {
            base.Attack();
            
            //Debug.Log(_enemies.transform.childCount);
            if (_attackDurationCounter <= 0 && _attackDelayCounter <= 0)
            {
                _currentAttack = RollAttack();
                _attackDurationCounter = _attackDuration;
            }

            if (_attackDelayCounter > 0)
            {
                _attackDelayCounter -= Time.fixedDeltaTime;
            }

            if (_summonDelayCounter <= 0)
            {
                _summonDelayCounter = _summonDelay;
            }
            else if (_summonDelayCounter <= 0 && _enemies.transform.childCount < _maxGators + 1)
            {
                SpawnGators(_maxGators + 1 - _enemies.transform.childCount);
                _summonDelayCounter = _summonDelay;
            }
            
            if (_summonDelayCounter > 0) _summonDelayCounter -= Time.fixedDeltaTime;
            
            switch (_currentAttack)
            {
                case SobekAttacks.Charge:
                    PerformChargeAttack();
                    break;
                case SobekAttacks.Spiral:
                    PerformSpiralAttack();
                    break;
                case SobekAttacks.RandomMoving:
                    PerformRandomMovingAttack();
                    break;
            }
            
        }

        private void SpawnGators(int number)
        {
            for (int i = 0; i < number; i++)
            {
                _spawner.SpawnAtRandomPosition();
            }
        }
        
        private SobekAttacks RollAttack()
        {
            int attackNumber = Random.Range(0, 3);

            switch (attackNumber)
            {
                case 0:
                    //_charging = true;
                    _attackDuration = 1000;
                    return SobekAttacks.Charge;
                case 1:
                    _attackDuration = _spiralDuration;
                    _spiralAngle = 0f;
                    return SobekAttacks.Spiral;
                case 2:
                    _attackDuration = _movingDuration;
                    return SobekAttacks.RandomMoving;
            }

            return SobekAttacks.RandomMoving;
        }

        protected override void OnHpChangedHandler(int hp, int maxHp)
        {
            base.OnHpChangedHandler(hp, maxHp);
            if (hp <= maxHp/2)
            {
                _rainSpawner.SetActive(true);
            }

            if (hp <= 0)
            {
                Destroy(_rainSpawner);
            }
        }
        
        private void Shoot8()
        {
            if (_shootCounter <= 0f)
            {
                _shootCounter = EnemyStats.GetFireRate();

                Vector2[] dirs =
                {
                    Vector2.up,
                    Vector2.down,
                    Vector2.left,
                    Vector2.right,
                    new Vector2(1, 1).normalized,
                    new Vector2(-1, 1).normalized,
                    new Vector2(1, -1).normalized,
                    new Vector2(-1, -1).normalized
                };

                foreach (var dir in dirs)
                {
                    projectileFactory.Shoot(dir);
                }
            }

            _shootCounter -= Time.fixedDeltaTime;
        }
        
        private void ShootSpiral()
        {
            if (_shootCounter <= 0f)
            {
                _shootCounter = _spiralFireRate;
                
                Vector2[] dirs =
                {
                    Vector2.up,
                    Vector2.down,
                    Vector2.left,
                    Vector2.right
                };

                foreach (var dir in dirs)
                {
                    Vector2 rotated = Rotate(dir, _spiralAngle);
                    projectileFactory.Shoot(rotated);
                }

                _spiralAngle += 10;
            }
            
            _shootCounter -= Time.fixedDeltaTime;
        }

        private Vector2 Rotate(Vector2 v, float angle)
        {
            return Quaternion.Euler(0, 0, angle) * v;
        }

        
        
        private void MoveToRandomDirection()
        {
            _posChangeCounter -= Time.fixedDeltaTime;
            
            if (_posChangeCounter <= 0f)
            {
                PickNewRandomTarget360();
                _posChangeCounter = _posChangeInterval;
            }
            
            MoveTo(_currentRandomTarget);
        }

        private void PerformRandomMovingAttack()
        {
            if (_attackDurationCounter <= 0)
            {
                _attackDelayCounter = EnemyStats.GetAtkSpd();
            }
            else
            {
                MoveToRandomDirection();
                Shoot8();
                _attackDurationCounter -= Time.fixedDeltaTime;
            }
        }

        private void PerformSpiralAttack()
        {
            if (_attackDurationCounter <= 0)
            {
                _attackDelayCounter = EnemyStats.GetAtkSpd();
            }
            else
            {
                ShootSpiral();
                _attackDurationCounter -= Time.fixedDeltaTime;
            }
        }

        private void PerformChargeAttack()
        {
            if (!_charging)
            {
                _windingUpCounter -= Time.fixedDeltaTime;
                if (_windingUpCounter <= 0)
                {
                    StartCharge();
                }
                return;
            }
        
            if (_miniChargeCounter > 0)
            {
                _miniChargeCounter -= Time.fixedDeltaTime;
                return;
            }
        
            if (_chargesNumber > 0)
            {
                MoveInDirection(_chargeDirection);
            }
        
            if (_chargesNumber <= 0)
            {
                _charging = false;
                EnemyStats.SetMovementSpeed(_originalSpeed);
                _windingUpCounter = _windUpTime;
                _attackDurationCounter = 0;
                _attackDelayCounter = EnemyStats.GetAtkSpd();
            }
        }
        
        
        private void StartCharge()
        {
            _charging = true;
            EnemyStats.SetMovementSpeed(_originalSpeed * _chargingSpeedMult);
            _chargesNumber = RollChargesNumber();
            _chargeDirection = (_player.transform.position - transform.position).normalized;
            FlipTo(_chargeDirection.x);
            _miniChargeCounter = 0f;
        }
        
        private int RollChargesNumber()
        {
            return Random.Range(1, 4);
        }
        
        private void PickNewRandomTarget360()
        {
            float angle = Random.Range(0f, 360f);
            
            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ).normalized;
            
            _currentRandomTarget = (Vector2)_rb.position + dir * _randomMoveDistance;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((!other.gameObject.CompareTag("Obstacle") && !other.gameObject.CompareTag("Player")) || !_charging) return;
            _chargesNumber--;
            _chargeDirection = (_player.transform.position - transform.position).normalized;
            _miniChargeCounter = _miniChargeCooldown;
        }
    }
}