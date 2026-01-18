using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class AnkhSpawner : EnemyProjectileFactory
{
    [SerializeField] private List<GameObject> LeftSpawners;
    [SerializeField] private List<GameObject> RightSpawners;
    private bool _spawnRight = true;
    private int _spawnIndex = 0;
    private Vector2 spawnPos;
    
    [SerializeField] private float spawnDelay = 10f;
    private float _timer;
    
    private void FixedUpdate()
    {
        _timer -= Time.fixedDeltaTime;

        if (_timer <= 0f)
        {
            Spawn();
            _timer = spawnDelay;
        }
    }

    private void Spawn()
    {
        _spawnIndex = GetRandomIndex();
        
        if (_spawnRight)
        {
            spawnPos = RightSpawners[_spawnIndex].transform.position;
            IPoolableObject projectile = _projectilePool.GetObject();
            projectile.Spawn(spawnPos,Vector2.left);
            projectile.Reparent(gameObject);
            _spawnRight = false;
        }
        else
        {
            spawnPos = LeftSpawners[_spawnIndex].transform.position;
            IPoolableObject projectile = _projectilePool.GetObject();
            projectile.Spawn(spawnPos,Vector2.right);
            projectile.Reparent(gameObject);
            _spawnRight = true;
        }
    }

    private int GetRandomIndex()
    {
        return Random.Range(0, 3);
    }
}
