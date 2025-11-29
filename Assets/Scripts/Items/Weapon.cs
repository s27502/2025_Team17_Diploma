using System.Collections.Generic;
using DefaultNamespace;
using Managers;
using Player;
using UnityEngine;

namespace Items
{
    public class Weapon : Item , IInteractable
    {
        [SerializeField] private GameObject _projectile;
        private PlayerStats _stats;
        private Equipment _equipment;
        
        
        public void OnInteract()
        {
            _stats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
            _equipment = ServiceLocator.Instance.GetService<EquipmentManager>().GetEquipment();
            Item toDrop;
            if (IsShop)
            {
                //Buy
                if (_stats.GetCoins() > GetItemPrice())
                {
                    _stats.ModifyCoins(-GetItemPrice());
                }
                toDrop = _equipment.Equip(this);
                if (toDrop)
                {
                    DeEquipStatChanges(toDrop);
                    ItemManager.PutInRoom(toDrop, gameObject.transform.position);
                }

                ApplyStatChanges(this);
                return;
            }
            //Pick up
            toDrop = _equipment.Equip(this);
            if (toDrop)
            {
                DeEquipStatChanges(toDrop);
                ItemManager.PutInRoom(toDrop, gameObject.transform.position);
            }
            ApplyStatChanges(this);
            ItemManager.PutInStorage(this);
        }

        private void ApplyStatChanges(Item item)
        {
            List<float> stats = item.GetStats();
            
            if (stats[0] != 0)
            {
                _stats.ModifyDmg((int)stats[0]);
            }

            if (stats[1] != 0)
            {
                _stats.ModifyAttackSpeed(stats[1]);
            }
        }

        private void DeEquipStatChanges(Item item)
        {
            List<float> stats = item.GetStats();
            
            if (stats[0] != 0)
            {
                _stats.ModifyDmg(-(int)stats[0]);
            }

            if (stats[1] != 0)
            {
                _stats.ModifyAttackSpeed(-stats[1]);
            }
        }
        public GameObject GetProjectile()
        {
            return _projectile;
        }
    }
}
