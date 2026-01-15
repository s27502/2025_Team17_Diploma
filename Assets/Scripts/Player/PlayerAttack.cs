using System.Collections.Generic;
using Items;
using Managers;
using Player;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _defaultProjectile;
    private GameObject _projectile;
    private IObjectPool _projectilePool;
    private IObjectFactory _factory;
    [SerializeField] private AudioClip playerShoot;
    
    [HideInInspector] public List<GameObject> EnemiesInRange = new List<GameObject>();

    private PlayerStats _stats;
    private float attackTimer = 0f;

    [SerializeField] private float _projectileOffset = 20f;
    private float _offsetStart;

    private void Start()
    {
        _stats = GetComponent<PlayerStats>();

        var equipment = ServiceLocator.Instance.GetService<EquipmentManager>().GetEquipment();
        equipment.OnWeaponChanged += UpdateWeapon;
        
        if (equipment.GetWeapon() is Weapon w)
            UpdateWeapon(w);
        else
            SetDefaultProjectile();
    }

    public void UpdateWeapon(Weapon newWeapon)
    {
        if (newWeapon == null)
        {
            SetDefaultProjectile();
            return;
        }

        _projectile = newWeapon.GetProjectile();
        RebuildProjectilePool();
    }
    
    private void SetDefaultProjectile()
    {
        _projectile = _defaultProjectile;
        RebuildProjectilePool();
    }

    public void RebuildProjectilePool()
    {
        _factory = new ProjectileFactory(_projectile);
        _projectilePool = new ProjectilePool(_factory, 25);
    }


    private void FixedUpdate()
    {
        if (EnemiesInRange.Count == 0)
            return;
        
        attackTimer += Time.fixedDeltaTime;

        float attackInterval = 1f / _stats.GetAtkSpeed();

        if (attackTimer >= attackInterval)
        {
            attackTimer -= attackInterval;
            GameObject target = GetClosestEnemy();
            if (target != null)
            {
                AudioManager.Instance.PlaySfx(playerShoot);
                Attack(target);
            }
        }
    }
    

    private void Attack(GameObject target)
    {
        int count = _stats.GetProjectileCount();
        _offsetStart = -(_projectileOffset * (count - 1) / 2f);
        
        Vector2 baseDir = (target.transform.position - transform.position).normalized;

        for (int i = 0; i < count; i++)
        {
            IPoolableObject projectile = _projectilePool.GetObject();
            if (projectile == null) continue;

            Vector2 rotatedDir = Rotate(baseDir, _offsetStart).normalized;

            Vector2 spawnPos = (Vector2)transform.position + rotatedDir * 0.7f;
            
            Vector2 finalDir = (target.transform.position - (Vector3)spawnPos).normalized;

            projectile.Spawn(spawnPos, finalDir);

            _offsetStart += _projectileOffset;
        }
    }


    
    private Vector2 Rotate(Vector2 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }



    public GameObject GetClosestEnemy()
    {
        EnemiesInRange.RemoveAll(e => e == null);
        
        if (EnemiesInRange.Count == 0)
            return null;

        GameObject closest = EnemiesInRange[0];
        float closestDistance = Vector3.Distance(transform.position, closest.transform.position);

        foreach (GameObject enemy in EnemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closest = enemy;
                closestDistance = distance;
            }
        }

        return closest;
    }
}