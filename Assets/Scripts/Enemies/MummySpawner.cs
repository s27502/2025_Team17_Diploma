using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Factory;
using UnityEngine;

public class MummySpawner : EnemySpawner
{
    [SerializeField] private GameObject _enemyPrefab2;

    public void SpawnAtRandomPosition(bool poison)
    {
        if (poison)
        {
            SpawnAtRandomPosition2();
        }
        else
        {
            SpawnAtRandomPosition1();
        }
    }
    
    public Enemy SpawnAtRandomPosition1()
    {
        Vector2 randomOffset = new Vector2(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
        );

        Vector2 spawnPos = (Vector2)areaCenter.position + randomOffset;
        Vector3 finalPos = new Vector3(spawnPos.x, spawnPos.y, 10.0411f);
        GameObject enemy = Instantiate(_enemyPrefab, finalPos, Quaternion.identity, transform);
        enemy.transform.parent = _enemies.transform;
        return enemy.GetComponent<Enemy>();
    }
    
    public Enemy SpawnAtRandomPosition2()
    {
        Vector2 randomOffset = new Vector2(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
        );

        Vector2 spawnPos = (Vector2)areaCenter.position + randomOffset;
        Vector3 finalPos = new Vector3(spawnPos.x, spawnPos.y, 10.0411f);
        GameObject enemy = Instantiate(_enemyPrefab2, finalPos, Quaternion.identity, transform);
        enemy.transform.parent = _enemies.transform;
        return enemy.GetComponent<Enemy>();
    }
}
