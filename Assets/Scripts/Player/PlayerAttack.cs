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
    
    [HideInInspector] public List<GameObject> EnemiesInRange = new List<GameObject>();

    private PlayerStats _stats;
    private float attackTimer = 0f;

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

    private void RebuildProjectilePool()
    {
        _factory = new ProjectileFactory(_projectile);
        _projectilePool = new ProjectilePool(_factory, 5);
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
                Attack(target);
            }
        }
    }
    

    private void Attack(GameObject target)
    {
        IPoolableObject projectile = _projectilePool.GetObject();
        if (projectile != null)
        {
            Vector2 dir = (target.transform.position - transform.position).normalized;
            
            Vector2 spawnPos = (Vector2)transform.position + dir * 3f;

            projectile.Spawn(spawnPos, dir);
        }
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