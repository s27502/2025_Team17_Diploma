using System.Collections.Generic;
using DefaultNamespace.Factory;
using UnityEngine;

public enum SobekAttacks
{
    Charge,
    Shoot,
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
        private float _summonDelayCounter;

        [Header("Shoot Attack (from Ra)")] 
        [SerializeField] private float _shootDuration;
        private float _shootCounter;
        private Vector2 _shootingDir;
        private bool _shootStarted;
        private bool _shootEnded;

        [SerializeField] private GameObject _rainSpawner;
        [SerializeField] private GameObject _enemySpawnerObject;
        private EnemySpawner _spawner;

        private SobekAttacks _currentAttack;

        private float _attackDurationCounter;
        private float _attackDuration;
        private float _attackDelayCounter;

        private float _windingUpCounter;
        private bool _charging;
        private int _chargesNumber;
        private Vector2 _chargeDirection;
        private float _originalSpeed;
        private float _miniChargeCounter;

        [Header("Random Move")]
        private Vector2 _currentRandomTarget;
        [SerializeField] private float _posChangeInterval = 0.5f;
        private float _posChangeCounter;
        [SerializeField] private float _randomMoveDistance = 2f;
        [SerializeField] private float _movingDuration = 5f;

        [SerializeField] private int _maxGators = 2;

        protected override void Start()
        {
            base.Start();
            _spawner = _enemySpawnerObject.GetComponent<EnemySpawner>();

            _attackDurationCounter = 0;
            _attackDelayCounter = 0;
            _summonDelayCounter = 15;

            _windingUpCounter = _windUpTime;
            _originalSpeed = EnemyStats.GetMovementSpeed();
        }

        protected override void Attack()
        {
            base.Attack();

            if (_attackDurationCounter <= 0 && _attackDelayCounter <= 0)
            {
                _currentAttack = RollAttack();
                _attackDurationCounter = _attackDuration;
            }

            if (_attackDelayCounter > 0)
                _attackDelayCounter -= Time.fixedDeltaTime;

            HandleSummoning();

            switch (_currentAttack)
            {
                case SobekAttacks.Charge:
                    PerformChargeAttack();
                    break;
                case SobekAttacks.Shoot:
                    PerformShootAttack();
                    break;
                case SobekAttacks.RandomMoving:
                    PerformRandomMovingAttack();
                    break;
            }
        }
        
        private void HandleSummoning()
        {
            if (_summonDelayCounter <= 0 && _enemies.transform.childCount < _maxGators + 1)
            {
                SpawnGators(_maxGators + 1 - _enemies.transform.childCount);
                _summonDelayCounter = _summonDelay;
            }

            if (_summonDelayCounter > 0)
                _summonDelayCounter -= Time.fixedDeltaTime;
        }

        private void SpawnGators(int number)
        {
            for (int i = 0; i < number; i++)
                _spawner.SpawnAtRandomPosition();
        }
        
        private SobekAttacks RollAttack()
        {
            int roll = Random.Range(0, 3);
            
            switch (roll)
            {
                case 0:
                    _attackDuration = 1000f;
                    return SobekAttacks.Charge;

                case 1:
                    _attackDuration = 2f;
                    _shootCounter = _shootDuration;
                    _shootStarted = false;
                    _shootEnded = false;
                    return SobekAttacks.Shoot;

                case 2:
                    _attackDuration = _movingDuration;
                    return SobekAttacks.RandomMoving;
            }

            return SobekAttacks.RandomMoving;
        }
        
        protected override void OnHpChangedHandler(int hp, int maxHp)
        {
            base.OnHpChangedHandler(hp, maxHp);

            if (hp <= maxHp / 2)
                _rainSpawner.SetActive(true);

            if (hp <= 0)
                Destroy(_rainSpawner);
        }
        


        private void PerformShootAttack()
        {
            if (!_shootStarted)
            {
                Shoot3();
                _shootStarted = true;
            }
            
            _shootCounter -= Time.fixedDeltaTime;

            if (_shootCounter <= 0 && !_shootEnded)
            {
                Shoot4();
                _shootEnded = true;
            }

            if (_attackDurationCounter <= 0)
            {
                _attackDelayCounter = EnemyStats.GetAtkSpd();
                return;
            }

            _attackDurationCounter -= Time.fixedDeltaTime;
        }


        private void Shoot4()
        {
            if (_player)
                _shootingDir = (_player.transform.position - transform.position).normalized;

            projectileFactory.Shoot(Rotate(_shootingDir, -20));
            projectileFactory.Shoot(Rotate(_shootingDir, 20));
            projectileFactory.Shoot(Rotate(_shootingDir, -60));
            projectileFactory.Shoot(Rotate(_shootingDir, 60));
        }

        private Vector2 Rotate(Vector2 v, float angle)
        {
            return Quaternion.Euler(0, 0, angle) * v;
        }

        private void PerformRandomMovingAttack()
        {
            if (_attackDurationCounter <= 0)
            {
                _attackDelayCounter = EnemyStats.GetAtkSpd();
                return;
            }

            MoveToRandomDirection();
            Shoot8();
            _attackDurationCounter -= Time.fixedDeltaTime;
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

        private void PickNewRandomTarget360()
        {
            float angle = Random.Range(0f, 360f);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
            _currentRandomTarget = (Vector2)_rb.position + dir * _randomMoveDistance;
        }

        private void Shoot8()
        {
            if (_shootCounter <= 0f)
            {
                _shootCounter = EnemyStats.GetFireRate();

                Vector2[] dirs =
                {
                    Vector2.up, Vector2.down, Vector2.left, Vector2.right,
                    new Vector2(1,1).normalized, new Vector2(-1,1).normalized,
                    new Vector2(1,-1).normalized, new Vector2(-1,-1).normalized
                };

                foreach (var dir in dirs)
                    projectileFactory.Shoot(dir);
            }

            _shootCounter -= Time.fixedDeltaTime;
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
                _windingUpCounter = _windUpTime;
                _attackDurationCounter = 0;
                _attackDelayCounter = EnemyStats.GetAtkSpd();
            }
        }

        private void StartCharge()
        {
            _charging = true;
            EnemyStats.SetMovementSpeed(_originalSpeed * _chargingSpeedMult);
            _chargesNumber = Random.Range(1, 4);
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
}
