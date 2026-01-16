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
    OmegaShoot
}
public class Anubis : Boss
{
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
    private EnemySpawner _spawner;



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
    private int _maxMummies = 2;

    private void Awake()
    {
        InitiateCornerList();
        _spawner = _enemySpawnerObject.GetComponent<EnemySpawner>();
        _summonDelayCounter = 5;
    }

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
                case AnubisAttacks.SpikeSpawn:
                    PerformSpikeSpawn();
                    Shoot8();
                    break;
                case AnubisAttacks.ChaseShoot:
                    PerformChaseShoot();
                    break;
            }
        }
        
        HandleSummoning();
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
            _spawner.SpawnAtRandomPosition();
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
        Debug.Log(distance);
        
        if (distance > 0.1f)
        {
            MoveTo(_cornerPositions[_currentCorner]);
        }
        else
        {
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
        _goToCorner = true;
        _currentCorner = GetFurthestCornerIndex();
        _rollNewAttack = false;
        _delaySpikesCounter = 0f;
        _spikeLifetimeCounter = 0f;
        _shootCounter = 0f;
    }

    private void SetUpChaseShoot()
    {
        _shoot3 = true;
        _attackDuration = chaseAttackDuration;
    }
    
    private void DelayNextAttack()
    {
        //idle anim
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

        if (val >= 101) //80
        {
            SetUpSpikeAttack();
            return AnubisAttacks.SpikeSpawn;
        }

        if (val >= 0) //
        {
            SetUpChaseShoot();
            return AnubisAttacks.ChaseShoot;
        }
        
        SetUpChaseShoot();
        return AnubisAttacks.ChaseShoot;
    }

}
