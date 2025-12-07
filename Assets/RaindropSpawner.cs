using Enemies;
using UnityEngine;

public class RaindropSpawner : EnemyProjectileFactory
{
    [SerializeField] private float spawnDelay = 1f;   // 1 sekunda
    private float _timer;

    private void FixedUpdate()
    {
        _timer -= Time.fixedDeltaTime;

        if (_timer <= 0f)
        {
            Spawn();
            _timer = spawnDelay;    // reset timera
        }
    }
}