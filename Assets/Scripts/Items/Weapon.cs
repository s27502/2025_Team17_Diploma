using DefaultNamespace;
using UnityEngine;

namespace Items
{
    public class Weapon : Item , IInteractable
    {
        [SerializeField] private GameObject _projectile;
        private bool _isShop;
        
        public void OnInteract()
        {
            if (_isShop)
            {
                //Buy
                return;
            } 
            //Pick up
        }
        
        public GameObject GetProjectile()
        {
            return _projectile;
        }
    }
}
