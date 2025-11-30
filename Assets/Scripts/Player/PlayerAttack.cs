using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    private IObjectPool _projectilePool;
    
    [HideInInspector] public List<GameObject> EnemiesInRange = new List<GameObject>();

    private PlayerStats _stats;
    private float attackTimer = 0f;

    private void Start()
    {
        _stats = GetComponent<PlayerStats>();
        IObjectFactory factory = new ProjectileFactory(_projectile);
        _projectilePool = new ProjectilePool(factory, 5);
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
        Debug.Log("Attacking: " + target.name);
        IPoolableObject projectile = _projectilePool.GetObject();
        if (projectile != null)
        {
            Vector2 dir = (target.transform.position - transform.position).normalized;
            projectile.Spawn(transform.position, dir);
        }
    }


    public GameObject GetClosestEnemy()
    {
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