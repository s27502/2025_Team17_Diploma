using UnityEngine;

namespace Enemies
{
    public class EnemyProjectileFactory : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int initialPoolSize = 10;

        protected IObjectPool _projectilePool;

        private void Awake()
        {
            IObjectFactory factory = new ProjectileFactory(projectilePrefab);
            _projectilePool = new ProjectilePool(factory, initialPoolSize);
        }

        public void Shoot(Vector2 direction)
        {
            if (direction == Vector2.zero)
                return;

            direction.Normalize();

            IPoolableObject projectile = _projectilePool.GetObject();

            if (projectile != null)
            {
                projectile.Spawn(transform.position, direction);
            }
        }

        public void ShootFromPosition(Vector2 direction, Vector2 position)
        {
            if (direction == Vector2.zero)
                return;

            direction.Normalize();

            IPoolableObject projectile = _projectilePool.GetObject();

            if (projectile != null)
            {
                projectile.Spawn(position, direction);
            }
        }
    }
}