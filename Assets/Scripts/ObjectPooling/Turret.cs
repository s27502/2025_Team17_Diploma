using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 1.5f;

    private IObjectPool _projectilePool;
    private Coroutine _shootingCoroutine;

    void Start() {
        IObjectFactory factory = new ProjectileFactory(projectilePrefab);
        _projectilePool = new ProjectilePool(factory, 10);
        ActivateShooting();
    }

    public void ActivateShooting() {
        if (_shootingCoroutine == null) {
            _shootingCoroutine = StartCoroutine(ShootingLoop());
        }
    }

    public void DeactivateShooting() {
        if (_shootingCoroutine != null) {
            StopCoroutine(_shootingCoroutine);
            _shootingCoroutine = null;
        }
    }

    private IEnumerator ShootingLoop() {
        while (true) {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Shoot() {
        IPoolableObject projectile = _projectilePool.GetObject();
        if (projectile != null) {
            projectile.Spawn(transform.position, transform.forward);
        }
    }

}