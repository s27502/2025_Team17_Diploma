using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    [SerializeField] private GameObject _projectile;

    public GameObject GetProjectile()
    {
        return _projectile;
    }
}
