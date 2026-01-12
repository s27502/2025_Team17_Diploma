using System;
using UnityEngine;
using Enemies;
using Managers;
using Player;
using StatusSystem;
using Random = UnityEngine.Random;

public class PlayerProjectile : ProjectileBase
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private float damageTextRange = 0.5f;
    

    [SerializeField] GameObject _splashProjectile;
    private IObjectFactory _factory;
    protected IObjectPool _splashProjectilePool;
    

    private PlayerStats _playerStats;
    private int _bounceNumber = 0;
    private IObjectPool _damagePopupPool;
    private IObjectFactory _damagePopupFactory;

    protected override void Awake()
    {
        base.Awake();

        _playerStats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
        _factory = new ProjectileFactory(_splashProjectile);
        _splashProjectilePool = new ProjectilePool(_factory, 10);

        if (damageTextPrefab != null)
        {
            _damagePopupFactory = new DamagePopupFactory(damageTextPrefab);
            _damagePopupPool = new DamagePopupPool(_damagePopupFactory, 20);
        }
    }

    public override void Spawn(Vector2 position, Vector2 direction)
    {
        base.Spawn(position,direction);
        _bounceNumber = 0;
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
                enemyStats.ModifyHp(-_playerStats.GetDmg());
                ShowDamageText(other.transform.position);

                if (_playerStats.GetPosion())
                {
                    var status = other.GetComponent<StatusHandler>();
                    status.Poison(
                        _playerStats.GetPoisonDuration(),
                        _playerStats.GetPoisonDMG()
                    );
                }
            }
        }
        
        if (_playerStats.GetHoming())
        {
            SpawnSplashingProjectiles();
        }
        
        if (_playerStats.GetBounce())
        {
            _rb.velocity = -_rb.velocity;
            _bounceNumber++;

            if (_bounceNumber > _playerStats.GetBounceNumber())
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
            popup.Setup(_playerStats.GetDmg());
        }
    }

    private void SpawnSplashingProjectiles()
    {
        Vector2 direction = _rb.velocity.normalized;

        Vector2 dir1 = new Vector2(-direction.y, direction.x);
        Vector2 dir2 = new Vector2(direction.y, -direction.x);
        
        IPoolableObject projectile1 = _splashProjectilePool.GetObject();
        projectile1.Spawn(transform.localPosition + Vector3.down * 0.2f,dir1);
        
        IPoolableObject projectile2 = _splashProjectilePool.GetObject();
        projectile2.Spawn(transform.localPosition + Vector3.down * 0.2f,dir2);
    }
}