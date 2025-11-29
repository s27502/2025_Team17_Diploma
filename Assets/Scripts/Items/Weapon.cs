using UnityEngine;

namespace Items
{
    public class Weapon : Item
    {
        [SerializeField] private GameObject _projectile;

        public GameObject GetProjectile()
        {
            return _projectile;
        }
    }
}
