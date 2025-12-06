using System.Collections.Generic;
using DefaultNamespace;
using Managers;
using Player;

namespace Items
{
    public class Talisman : Item , IInteractable
    {
        private PlayerStats _stats;
        private Inventory _inventory;
        
        
        public void OnInteract()
        {
            if (!_stats)
                _stats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
            if (!_inventory)
                _inventory = ServiceLocator.Instance.GetService<InventoryManager>().GetInventory();
            if (IsShop)
            {
                //Buy
                if (_stats.GetCoins() >= GetItemPrice())
                {
                    _stats.ModifyCoins(-GetItemPrice());
                    IsShop = false;
                    _inventory.AddItem(this);
                    ApplyStatChanges(this);
                    ItemManager.PutInStorage(this);
                }
                return;
            }
            //Pick up
            _inventory.AddItem(this);
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

            if (stats[2] != 0)
            {
                _stats.ModifyLuck((int)stats[2]);
            }
            if (stats[3] != 0)
            {
                _stats.ModifyMaxHp((int)stats[3]);
                _stats.ModifyHp((int)stats[3]);
            }
        }

        private void DeEquipStatChanges(Item item)
        {
            List<float> stats = item.GetStats();
            
            if (stats[0] != 0)
            {
                _stats.ModifyArmorMax(-(int)stats[0]);
                _stats.ModifyArmor(-(int)stats[0]);
            }

            if (stats[1] != 0)
            {
                _stats.ModifyAttackSpeed(-stats[1]);
            }
        }
    }
}
