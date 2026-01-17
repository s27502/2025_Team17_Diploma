using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Factory;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SnakeSpawner : EnemySpawner
{
    [SerializeField] private List<GameObject> spawnPositions;
    [SerializeField] private float spawnDelay = 3f;
    private float spawnCounter = 0f;
    private int spawnIndex = 0;

    private void FixedUpdate()
    {
        if (spawnCounter <= spawnDelay)
        {
            spawnCounter += Time.fixedDeltaTime;
        }
        else
        {
            spawnCounter = 0;
            spawnIndex = Random.Range(0, spawnPositions.Count);
            SpawnAt(spawnPositions[spawnIndex].transform.position);
        }
    }
}
