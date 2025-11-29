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
        private bool _isShop;
        
        public void OnInteract()
        {
            _stats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
            _equipment = ServiceLocator.Instance.GetService<EquipmentManager>().GetEquipment();
            Item toDrop;
            if (_isShop)
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
                    Debug.Log("Dropped " + toDrop.name);
                }

                ApplyStatChanges(this);
                return;
            }
            //Pick up
            toDrop = _equipment.Equip(this);
            if (toDrop)
            {
                DeEquipStatChanges(toDrop);
                Debug.Log("Dropped " + toDrop.name);
            }
            ApplyStatChanges(this);
            ServiceLocator.Instance.GetService<ItemManager>().PutInStorage(this);
        }

        private void ApplyStatChanges(Item item)
        {
            List<float> stats = item.GetStats();
            
            if (stats[2] != 0)
            {
                _stats.ModifyDmg((int)stats[2]);
            }

            if (stats[3] != 0)
            {
                _stats.ModifyAttackSpeed(stats[3]);
            }
        }

        private void DeEquipStatChanges(Item item)
        {
            List<float> stats = item.GetStats();
            
            if (stats[2] != 0)
            {
                _stats.ModifyDmg(-(int)stats[2]);
            }

            if (stats[3] != 0)
            {
                _stats.ModifyAttackSpeed(-stats[3]);
            }
        }
        public GameObject GetProjectile()
        {
            return _projectile;
        }
    }
}
