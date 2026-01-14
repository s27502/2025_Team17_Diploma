using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HieraFollower : Enemy
{
    [SerializeField] private GameObject MultiLaserSpawner;
    [SerializeField] private float LaserTime = 1f;
    [SerializeField] private float WindUpTime = 1f;
    private MultiLaserSpawner _spawner;

    private float _shootCounter = 0f;

    private void Awake()
    {
        _spawner = MultiLaserSpawner.GetComponent<MultiLaserSpawner>();
    }

    protected override void Attack()
    {
        MoveToPlayer();
        SnipePlayer();
    }
    
    private void SnipePlayer()
    {
        if (_shootCounter <= 0f)
        {
            _shootCounter = EnemyStats.GetFireRate();

            Vector2 dir = (_player.transform.position - transform.position).normalized;
            projectileFactory.Shoot(dir);
        }

        _shootCounter -= Time.fixedDeltaTime;
    }

    public void ActivateLaser(MultiLaserSpawner.LaserDirection dir)
    {
        StartCoroutine(ActivateLaserCoroutine(dir));
    }

    private IEnumerator ActivateLaserCoroutine(MultiLaserSpawner.LaserDirection dir)
    {
        yield return new WaitForSeconds(WindUpTime);
        _spawner.ShootForTimeInDir(LaserTime,dir);
    }
}
