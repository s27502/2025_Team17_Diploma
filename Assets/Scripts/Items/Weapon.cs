using System.Collections.Generic;
using DefaultNamespace;
using Managers;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Items
{
    public class Weapon : Item , IInteractable
    {
        [SerializeField] private GameObject _projectile;
        private PlayerStats _stats;
        private Equipment _equipment;

        private bool _isPoisonFromTalisman;
        private bool _isBouncingFromTalisman;
        private bool _isHomingFromTalisman;


        protected override void Start()
        {
            base.Start();
            
        }

        
        public void OnInteract()
        {
            if (!_stats)
                _stats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
            if (!_equipment)
                _equipment = ServiceLocator.Instance.GetService<EquipmentManager>().GetEquipment();
            Item toDrop;
            if (IsShop)
            {
                //Buy
                if (_stats.GetCoins() >= GetItemPrice())
                {
                    _price.SetActive(false);
                    _stats.ModifyCoins(-GetItemPrice());
                    IsShop = false;
                    toDrop = _equipment.Equip(this);
                    if (toDrop)
                    {
                        DeEquipStatChanges(toDrop);
                        ItemManager.PutInRoom(toDrop, gameObject.transform.position);
                    }
                    ApplyStatChanges(this);
                    ItemManager.PutInStorage(this);
                }
                return;
            }
            //Pick up
            toDrop = _equipment.Equip(this);
            if (toDrop)
            {
                DeEquipStatChanges(toDrop);
                ItemManager.PutInRoom(toDrop, gameObject.transform.position);
            }
            AudioManager.Instance.PlaySfx(pickUpSound);
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

            if (_stats.GetPosion())
            {
                _isPoisonFromTalisman = true;
            }
            else
            {
                _stats.SetPoison(_isPoisoning);
                _isPoisonFromTalisman = false;
            }
            if (_stats.GetBounce())
            {
                _isBouncingFromTalisman = true;
            }
            else
            {
                _stats.SetBounce(_isBouncing);
                _isBouncingFromTalisman = false;
            }
            if (_stats.GetHoming())
            {
                _isHomingFromTalisman = true;
            }
            else
            {
                _stats.SetHoming(_isHoming);
                _isHomingFromTalisman = false;
            }
        }

        private void DeEquipStatChanges(Item item)
        {
            Debug.Log("twoja matka");
            List<float> stats = item.GetStats();
            
            if (stats[0] != 0)
            {
                _stats.ModifyDmg(-(int)stats[0]);
            }

            if (stats[1] != 0)
            {
                _stats.ModifyAttackSpeed(-stats[1]);
            }

            if (!_isPoisonFromTalisman && _isPoisoning)
            {
                _stats.SetPoison(!_isPoisoning);
                _stats.SetBounce(_isBouncing);
                _stats.SetHoming(_isHoming);
            } else if (!_isBouncingFromTalisman && _isBouncing)
            {
                _stats.SetPoison(_isPoisoning);
                _stats.SetBounce(!_isBouncing);
                _stats.SetHoming(_isHoming);
            } else if (!_isHomingFromTalisman && _isHoming)
            {
                _stats.SetPoison(_isPoisoning);
                _stats.SetBounce(_isBouncing);
                _stats.SetHoming(!_isHoming);
            }
            else
            {
                _stats.SetPoison(_isPoisoning);
                _stats.SetBounce(_isBouncing);
                _stats.SetHoming(_isHoming);
            }
        }
        public GameObject GetProjectile()
        {
            return _projectile;
        }
    }
}
