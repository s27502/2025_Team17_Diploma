using Enemies;
using Managers;
using Player;
using StatusSystem;
using UnityEngine;

namespace ObjectPooling.Projectiles
{
    public class PlayerSplashProjectile : ProjectileBase
    {
        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private float damageTextRange = 0.5f;
        
        private IObjectPool _damagePopupPool;
        private IObjectFactory _damagePopupFactory;

        protected override void Awake()
        {
            base.Awake();

            if (damageTextPrefab != null)
            {
                _damagePopupFactory = new DamagePopupFactory(damageTextPrefab);
                _damagePopupPool = new DamagePopupPool(_damagePopupFactory, 20);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                return;

            bool hitEnemy = other.CompareTag("Enemy");

            if (hitEnemy)
            {
                var enemyStats = other.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.ModifyHp(-1);
                    ShowDamageText(other.transform.position);
                }
            }
            
            _pool?.ReleaseObject(this);
        }


        private void ShowDamageText(Vector3 enemyPos)
        {
            if (_damagePopupPool == null)
                return;

            Vector3 randomOffset = new Vector3(
                Random.Range(-damageTextRange, damageTextRange),
                Random.Range(-damageTextRange, damageTextRange),
                0f
            );

            IPoolableObject popupObj = _damagePopupPool.GetObject();
            if (popupObj != null)
            {
                popupObj.Spawn(enemyPos + randomOffset, Vector2.zero);

                DamagePopup popup = popupObj as DamagePopup;
                popup.Setup(1);
            }
        }
    }
}