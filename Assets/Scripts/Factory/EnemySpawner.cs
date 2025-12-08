using UnityEngine;

namespace DefaultNamespace.Factory
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private GameObject _enemies;
        
        [Header("Spawn Area")]
        [SerializeField] private Vector2 areaSize = new Vector2(30f, 20f);
        [SerializeField] private Transform areaCenter;

        public void SpawnAtRandomPosition()
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
            );

            Vector2 spawnPos = (Vector2)areaCenter.position + randomOffset;
            Vector3 finalPos = new Vector3(spawnPos.x, spawnPos.y, 10.0411f);
            GameObject enemy = Instantiate(_enemyPrefab, finalPos, Quaternion.identity, transform);
            enemy.transform.parent = _enemies.transform;

        }
    }
}