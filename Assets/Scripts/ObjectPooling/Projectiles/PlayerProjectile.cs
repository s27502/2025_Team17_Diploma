using UnityEngine;
using Enemies;
using Managers;
using Player;
using StatusSystem;

public class PlayerProjectile : ProjectileBase
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private float damageTextRange = 0.5f;

    private PlayerStats _playerStats;
    private int _bounceNumber = 0;
    private IObjectPool _damagePopupPool;
    private IObjectFactory _damagePopupFactory;


    protected override void Awake()
    {
        base.Awake();

        _playerStats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();

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

        if (!_playerStats.GetBounce() && !_playerStats.GetPosion() && !_playerStats.GetHoming())
        {
            if (other.CompareTag("Enemy"))
            {
                var enemyStats = other.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.ModifyHp(-_playerStats.GetDmg());
                    ShowDamageText(other.transform.position);
                }
                _pool?.ReleaseObject(this);
                return;
            }

            _pool?.ReleaseObject(this);
        }

        if (_playerStats.GetBounce())
        {
            _rb.velocity = -_rb.velocity;
            _bounceNumber++;
            if (other.CompareTag("Enemy"))
            {
                var enemyStats = other.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.ModifyHp(-_playerStats.GetDmg());
                    ShowDamageText(other.transform.position);

                    if (_playerStats.GetPosion())
                    {
                        var enemyStatusHandler = other.GetComponent<StatusHandler>();
                        enemyStatusHandler.Poison(_playerStats.GetPoisonDuration(),_playerStats.GetPoisonDMG());
                    }
                }
                if (_bounceNumber > _playerStats.GetBounceNumber())
                {
                    _pool?.ReleaseObject(this);
                }
                return;
            }
            
            if (_bounceNumber > _playerStats.GetBounceNumber())
            {
                _pool?.ReleaseObject(this);
            }
        }

        if (_playerStats.GetPosion())
        {
            if (other.CompareTag("Enemy"))
            {
                var enemyStats = other.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.ModifyHp(-_playerStats.GetDmg());
                    ShowDamageText(other.transform.position);
                    var enemyStatusHandler = other.GetComponent<StatusHandler>();
                    enemyStatusHandler.Poison(_playerStats.GetPoisonDuration(),_playerStats.GetPoisonDMG());
                }
                _pool?.ReleaseObject(this);
                return;
            }

            _pool?.ReleaseObject(this);
        }
        
        

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

}