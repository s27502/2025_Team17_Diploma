using UnityEngine;

namespace ObjectPooling.Projectiles
{
    public class Raindrop : TimedEnemyProjectile
    {
        [SerializeField] private float gravityScale = 1f;

        protected override void Awake()
        {
            base.Awake();
            _rb.gravityScale = gravityScale;
        }

        public override void Spawn(Vector2 position, Vector2 direction)
        {
            transform.position = position;
            _rb.velocity = Vector2.zero;
            gameObject.SetActive(true);
        }
    }
}