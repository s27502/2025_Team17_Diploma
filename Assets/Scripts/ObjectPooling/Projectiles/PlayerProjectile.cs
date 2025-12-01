using UnityEngine;
using Enemies;
using Managers;
using Player;

public class PlayerProjectile : ProjectileBase
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private float damageTextRange = 0.5f;
    
    private IObjectPool _damagePopupPool;
    private IObjectFactory _damagePopupFactory;


    protected override void Awake()
    {
        base.Awake();
    
        damage = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats().GetDmg();

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

        if (other.CompareTag("Enemy"))
        {
            var enemyStats = other.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.ModifyHp(-damage);
                ShowDamageText(other.transform.position);
            }
            _pool?.ReleaseObject(this);
            return;
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
            popup.Setup(damage);
        }
    }

}