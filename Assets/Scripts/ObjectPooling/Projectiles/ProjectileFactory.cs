using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory :  IObjectFactory
{
    private GameObject bulletPrefab;

    public ProjectileFactory(GameObject prefab) {
        bulletPrefab = prefab;
    }

    public IPoolableObject CreateNew() {
        GameObject obj = GameObject.Instantiate(bulletPrefab);
        obj.SetActive(false);
        return obj.GetComponent<IPoolableObject>();
    }
}
