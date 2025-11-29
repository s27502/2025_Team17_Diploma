using DefaultNamespace;
using Managers;
using Player;
using UnityEngine;

namespace Items
{
    public class Weapon : Item , IInteractable
    {
        [SerializeField] private GameObject _projectile;
        private bool _isShop;
        
        public void OnInteract()
        {
            PlayerStats stats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
            Equipment equipment = ServiceLocator.Instance.GetService<EquipmentManager>().GetEquipment();
            Item toDrop;
            if (_isShop)
            {
                //Buy
                if (stats.GetCoins() > GetItemPrice())
                {
                    stats.ModifyCoins(-GetItemPrice());
                }
                toDrop = equipment.Equip(this);
                if (toDrop)
                {
                    Debug.Log("Dropped " + toDrop.name);
                }
                return;
            }
            //Pick up
            toDrop = equipment.Equip(this);
            if (toDrop)
            {
                Debug.Log("Dropped " + toDrop.name);
            }
        }
        
        public GameObject GetProjectile()
        {
            return _projectile;
        }
    }
}
