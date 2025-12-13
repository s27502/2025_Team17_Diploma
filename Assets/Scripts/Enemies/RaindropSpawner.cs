using Enemies;
using UnityEngine;

public class RaindropSpawner : EnemyProjectileFactory
{
    [Header("Spawn Area")]
    [SerializeField] private Vector2 areaSize = new Vector2(5f, 1f);
    [SerializeField] private Transform areaCenter;

    [SerializeField] private float spawnDelay = 1f;
    private float _timer;

    private void FixedUpdate()
    {
        _timer -= Time.fixedDeltaTime;

        if (_timer <= 0f)
        {
            SpawnAtRandomPosition();
            _timer = spawnDelay;
        }
    }

    private void SpawnAtRandomPosition()
    {
        Vector2 randomOffset = new Vector2(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
        );

        Vector2 spawnPos = (Vector2)areaCenter.position + randomOffset;
        IPoolableObject projectile = _projectilePool.GetObject();
        projectile.Spawn(spawnPos,Vector2.down);
    }
}