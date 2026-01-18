using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Factory;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AnubisAttacks
{
    SpikeSpawn,
    ChaseShoot,
    DoubleSpiral,
    TripleBrimstone,
    OmegaShoot,
    MoveShoot
}
public class Anubis : Boss
{
    [SerializeField] private float moveShootDuration = 5f;
    [SerializeField] private GameObject ankhSpawner;
    
    private bool omega345 = false;
    private bool omegaWave = false;
    private int _currentShotNum;
    private bool _delay;
    private List<Vector2> _shootingDirections = new List<Vector2>();
    
    [SerializeField] private float spiralFireRate = 0.5f;
    private float spiralLeftCounter = 0f;
    private float spiralRightCounter = 0f;
    private float spiralLeftAngle;
    private float spiralRightAngle;
    
    [SerializeField] private List<GameObject> laserSpawners = new List<GameObject>();
    private int _brimstoneCounter = 0;

    [SerializeField] private float laserLifetime = 1.5f;

    [SerializeField] private float laserDelay = 1f;

    private bool _phase2 = true;

    private int _lasersFinishedCounter = 0;
    
    private float hiddenLaserDelay = .5f;

    [SerializeField] private float brimstoneDelay = .5f;
    private float _brimstoneDelayCouner = 0f;
    
    
    [SerializeField] private List<GameObject> cornerPositions = new List<GameObject>();
    private List<Vector2> _cornerPositions = new List<Vector2>();
    [SerializeField] private List<GameObject> spikeObjects;
    [SerializeField] private float spikeLifetime = 2f;
    private float _spikeLifetimeCounter = 0f;
    private bool _delaySpikes = false;
    [SerializeField] private float delaySpikesTime = 1f;
    private float _delaySpikesCounter = 0f;

    private Vector2 _shootingDir;
    private bool _shoot3 = true;
    private float _attackDelayCounter;
    
    [SerializeField] private float _summonDelay = 20f;
    
    [SerializeField] private GameObject _enemies;
    [SerializeField] private GameObject _enemySpawnerObject;
    private MummySpawner _spawner;



    private int _currentCorner;
    private bool _goToCorner = false;

    private float _shootCounter = 0f;

    private float _attackDuration = 0f;
    private float _attackDurationCounter = 0f;
    
    [SerializeField] private float chaseAttackDuration = 5f;
    [SerializeField] private float doubleSpiralDuration = 5f; 

    private bool _rollNewAttack = true;
    private AnubisAttacks _currentAttack;
    private float _summonDelayCounter;
    private bool _poison = false;
    private int _maxMummies = 1;

    private void Awake()
    {
        InitiateCornerList();
        _spawner = _enemySpawnerObject.GetComponent<MummySpawner>();
        _summonDelayCounter = 5;
    }

    protected override void Attack()
    {
        if (_rollNewAttack)
        {
            if (_phase2)
            {
                _currentAttack = RollAttack2();
            }
            else
            {
                _currentAttack = RollAttack();
            }
            
        }
        else
        {
            switch (_currentAttack)
            {
                case AnubisAttacks.SpikeSpawn:
                    PerformSpikeSpawn();
                    Shoot8();
                    break;
                case AnubisAttacks.ChaseShoot:
                    PerformChaseShoot();
                    break;
                case AnubisAttacks.TripleBrimstone:
                    PerformTripleBrimstone();
                    break;
                case AnubisAttacks.DoubleSpiral:
                    PerformDoubleSpiral();
                    break;
                case AnubisAttacks.OmegaShoot:
                    PerformOmegaShoot();
                    break;
                case AnubisAttacks.MoveShoot:
                    PerformMoveShoot();
                    break;
            }
        }
        
        HandleSummoning();
    }

    private AnubisAttacks RollAttack2()
    {
        int val = Random.Range(0, 100);
        _rollNewAttack = false;

        if (val >= 80)
        {
            SetUpMoveShoot();
            return AnubisAttacks.MoveShoot;
        }
        
        if (val >= 65) //80
        {
            SetUpSpikeAttack();
            return AnubisAttacks.SpikeSpawn;
        }

        if (val >= 45) //
        {
            SetUpChaseShoot();
            return AnubisAttacks.ChaseShoot;
        }

        if (val >= 25)
        {
            SetUpSpiral();
            return AnubisAttacks.DoubleSpiral;
        }

        if (val >= 15)
        {
            SetUpOmegaShoot();
            return AnubisAttacks.OmegaShoot;
        }

        SetUpTripleBrimstone();
        return AnubisAttacks.TripleBrimstone;
    }

    private void PerformMoveShoot()
    {
        if (_attackDurationCounter <= _attackDuration)
        {
            CrossMove();
            if (_shootCounter <= EnemyStats.GetFireRate())
            {
                _shootCounter += Time.fixedDeltaTime;
            }
            else
            {
                Shoot8();
                _shootCounter = 0f;
            }
            
            _attackDurationCounter += Time.fixedDeltaTime;
        }
        else
        {
            DelayNextAttack();
        }
    }
    
    private void Perform345Shoot()
    {
        if (_currentShotNum < 3)
        {
            if (_shootCounter <= EnemyStats.GetFireRate()/2)
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
            omegaWave = true;
            omega345 = false;
            _currentShotNum = 0;
        }
    }
    
    private void PerformWaveShoot()
    {
        if (_currentShotNum < 6)
        {
            if (_delay && _shootCounter <= EnemyStats.GetFireRate())
            {
                _shootCounter += Time.fixedDeltaTime;
            }
            else if (_shootCounter <= EnemyStats.GetFireRate()/2)
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
            omegaWave = false;
            DelayNextAttack();
        }
    }

    private void PerformOmegaShoot()
    {
        if (omega345)
        {
            Perform345Shoot();
        }
        else if (omegaWave)
        {
            PerformWaveShoot();
        }
        else
        {
            DelayNextAttack();
        }
    }

    private void PerformDoubleSpiral()
    {
        if (_attackDurationCounter <= _attackDuration)
        {
            SpiralAttack(true);
            SpiralAttack(false);
            
            _attackDurationCounter += Time.fixedDeltaTime;
        }
        else
        {
            DelayNextAttack();
        }
    }

    private void SpiralAttack(bool isRight)
    {
        switch (isRight)
        {
            case true:
                if (spiralRightCounter <= spiralFireRate)
                {
                    spiralRightCounter += Time.fixedDeltaTime;
                }
                else
                {
                    Vector2[] dirs =
                    {
                        Vector2.up,
                        Vector2.down,
                        Vector2.left,
                        Vector2.right
                    };

                    foreach (var dir in dirs)
                    {
                        projectileFactory.Shoot(RotateProjectile(dir, spiralRightAngle));
                    }

                    spiralRightAngle -= 20f;
                    spiralRightCounter = 0f;
                }
                break;
            case false:
                if (spiralLeftCounter <= spiralFireRate)
                {
                    spiralLeftCounter += Time.fixedDeltaTime;
                }
                else
                {
                    Vector2[] dirs =
                    {
                        Vector2.up,
                        Vector2.down,
                        Vector2.left,
                        Vector2.right
                    };

                    foreach (var dir in dirs)
                    {
                        projectileFactory.Shoot(RotateProjectile(dir, spiralLeftAngle));
                    }

                    spiralLeftAngle += 20f;
                    spiralLeftCounter = 0f;
                }
                break;
        }
    }

    private void PerformTripleBrimstone()
    {
        if (_brimstoneCounter < 3)
        {
            if (_brimstoneDelayCouner <= brimstoneDelay)
            {
                _brimstoneDelayCouner += Time.fixedDeltaTime;
            }
            else
            {
                StartCoroutine(ShootLaserCoroutine(laserSpawners[_brimstoneCounter]));

                _brimstoneDelayCouner = 0f;
                _brimstoneCounter++;
            }
        }
        else if (_lasersFinishedCounter == 3) 
        {
            DelayNextAttack();
        }
    }

    private IEnumerator ShootLaserCoroutine(GameObject laserSpawner)
    {
        yield return new WaitForSeconds(laserDelay);
        
        RotateLaserTowardsPlayer(laserSpawner);

        yield return new WaitForSeconds(hiddenLaserDelay);
        
        laserSpawner.SetActive(true);

        yield return new WaitForSeconds(laserLifetime);
        
        laserSpawner.SetActive(false);
        _lasersFinishedCounter++;
    }



    private void HandleSummoning()
    {
        if (_summonDelayCounter <= 0 && _enemies.transform.childCount < _maxMummies + 1)
        {
            SpawnMummies(_maxMummies + 1 - _enemies.transform.childCount);
            _summonDelayCounter = _summonDelay;
        }

        if (_summonDelayCounter > 0)
            _summonDelayCounter -= Time.fixedDeltaTime;
    }

    private void SpawnMummies(int number)
    {
        for (int i = 0; i < number; i++)
        {
            _spawner.SpawnAtRandomPosition(_poison);
            _poison = !_poison;
        }
    }

    private void PerformChaseShoot()
    {
        if (_attackDurationCounter <= _attackDuration)
        {
            //walk anim
            _attackDurationCounter += Time.fixedDeltaTime;
            MoveTo(_player.transform.position);
            if (_shootCounter <= EnemyStats.GetFireRate())
            {
                _shootCounter += Time.fixedDeltaTime;
            }
            else
            {
                ShootRotate34();
                _shootCounter = 0;
            }
        }
        else
        {
            DelayNextAttack();
        }
    }

    private void ShootRotate34()
    {
        if (_player)
        {
            _shootingDir = (_player.transform.position - transform.position).normalized;
        }

        if (_shoot3)
        {
            Shoot3();
            _shoot3 = false;
        }
        else
        {
            Shoot4();
            _shoot3 = true;
        }
    }

    private void Shoot8()
    {
        if (_shootCounter <= EnemyStats.GetFireRate())
        {
            _shootCounter += Time.fixedDeltaTime;
        }
        else
        {
            ShootCross();
            ShootX();
            _shootCounter = 0f;
        }
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
    
    private void ShootCross()
    {
        Vector2[] dirs =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        foreach (var dir in dirs)
            projectileFactory.Shoot(dir);
    }

    private void ShootX()
    {
        Vector2[] dirs =
        {
            new Vector2(1, 1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(-1, -1).normalized
        };

        foreach (var dir in dirs)
            projectileFactory.Shoot(dir);
    }

    private void PerformSpikeSpawn()
    {
        if (_goToCorner)
        {
            GoToCorner();
            //walk anim
        }
        else if (_delaySpikes)
        {
            //idle anim
            if (_delaySpikesCounter <= delaySpikesTime)
            {
                _delaySpikesCounter += Time.fixedDeltaTime;
            }
            else
            {
                _delaySpikes = false;
                spikeObjects[_currentCorner].SetActive(true);
            }
        }
        else
        {
            //staff anim
            if (_spikeLifetimeCounter <= spikeLifetime)
            {
                _spikeLifetimeCounter += Time.fixedDeltaTime;
            }
            else
            {
                spikeObjects[_currentCorner].SetActive(false);
                DelayNextAttack();
            }
        }
    }

    private void GoToCorner()
    {
        float distance = Vector2.Distance(transform.position, _cornerPositions[_currentCorner]);
        
        if (distance > 0.1f)
        {
            MoveTo(_cornerPositions[_currentCorner]);
        }
        else
        {
            _animator.SetBool("isWalking",false);
            _animator.SetBool("isCharging",true);
            _goToCorner = false;
            _delaySpikes = true;
        }
    }

    private void InitiateCornerList()
    {
        foreach (GameObject cornerPos in cornerPositions)
        {
            _cornerPositions.Add(cornerPos.transform.position);
        }
    }

    private int GetFurthestCornerIndex()
    {
        int furthestIndex = 0;
        float maxDistance = float.MinValue;

        Vector2 anubisPos = transform.position;

        for (int i = 0; i < _cornerPositions.Count; i++)
        {
            float distance = Vector2.Distance(anubisPos, _cornerPositions[i]);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestIndex = i;
            }
        }

        return furthestIndex;
    }

    private void SetUpSpikeAttack()
    {
        _animator.SetBool("isWalking",true);
        _goToCorner = true;
        _currentCorner = GetFurthestCornerIndex();
        _rollNewAttack = false;
        _delaySpikesCounter = 0f;
        _spikeLifetimeCounter = 0f;
        _shootCounter = 0f;
    }

    private void SetUpChaseShoot()
    {
        _animator.SetBool("isWalking",true);
        _shoot3 = true;
        _attackDuration = chaseAttackDuration;
    }
    
    private void DelayNextAttack()
    {
        //idle anim
        _animator.SetBool("isWalking",false);
        _animator.SetBool("isCharging",false);
        if (_attackDelayCounter <= EnemyStats.GetAtkSpd())
        {
            _attackDelayCounter += Time.fixedDeltaTime;
        }
        else
        {
            _rollNewAttack = true;
            _attackDelayCounter = 0f;
            _attackDurationCounter = 0f;
            _shootCounter = 0f;
            //_agent.enabled = true;
        }
    }

    private AnubisAttacks RollAttack()
    {
        int val = Random.Range(0, 100);
        _rollNewAttack = false;

        if (val >= 65)
        {
            SetUpMoveShoot();
            return AnubisAttacks.MoveShoot;
        }
        
        if (val >= 57) //80
        {
            SetUpSpikeAttack();
            return AnubisAttacks.SpikeSpawn;
        }

        if (val >= 37) //
        {
            SetUpChaseShoot();
            return AnubisAttacks.ChaseShoot;
        }

        if (val >= 25)
        {
            SetUpSpiral();
            return AnubisAttacks.DoubleSpiral;
        }

        if (val >= 15)
        {
            SetUpOmegaShoot();
            return AnubisAttacks.OmegaShoot;
        }

        SetUpTripleBrimstone();
        return AnubisAttacks.TripleBrimstone;
    }

    private void SetUpMoveShoot()
    {
        _animator.SetBool("isWalking",true);
        _attackDuration = moveShootDuration;
    }

    private void SetUpOmegaShoot()
    {
        _animator.SetBool("isWalking",false);
        _animator.SetBool("isCharging",false);
        omega345 = true;
        _shootingDirections.Clear();
        _shootCounter = 0;
        _currentShotNum = 0;
    }

    private void SetUpSpiral()
    {
        _animator.SetBool("isWalking",false);
        _animator.SetBool("isCharging",false);
        _attackDuration = doubleSpiralDuration;
        spiralLeftAngle = 0f;
        spiralLeftCounter = 0f;
        spiralRightAngle = 0f;
        spiralRightCounter = 0f;
    }

    private void SetUpTripleBrimstone()
    {
        _animator.SetBool("isCharging",true);
        _lasersFinishedCounter = 0;
        _brimstoneCounter = 0;
        _brimstoneDelayCouner = 0f;
    }

    private void RotateLaserTowardsPlayer(GameObject laserObject)
    {
        Vector2 dir = (_player.transform.position - laserObject.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        laserObject.transform.rotation = Quaternion.Euler(0f,0f, angle);
    }
    
    protected override void OnHpChangedHandler(int hp, int maxHp)
    {
        base.OnHpChangedHandler(hp, maxHp);

        if (hp <= maxHp / 2)
        {
            ankhSpawner.SetActive(true);
            _phase2 = true;
        }

        if (hp <= 0)
            Destroy(ankhSpawner);
    }


}
