using System;
using Items;
using UnityEngine;

namespace Player
{
    public class Equipment : MonoBehaviour
    {
        public event Action<Weapon> OnWeaponChanged;
        public event Action<Armor> OnArmorChanged;
        public event Action<Helmet> OnHelmetChanged;
        
        private Item _weapon;
        private Item _armor;
        private Item _helmet;

        public Item GetWeapon()
        {
            return _weapon;
        }

        public Item GetArmor()
        {
            return _armor;
        }

        public Item GetHelmet()
        {
            return _helmet;
        }

        public Item Equip(Item item)
        {
            Item toDrop = null;

            if (item is Weapon newWeapon)
            {
                if (_weapon)
                    toDrop = Unequip(_weapon);

                _weapon = newWeapon;
                
                OnWeaponChanged?.Invoke(newWeapon);
            } else if (item is Armor newArmor)
            {
                if (_armor)
                    toDrop = Unequip(_armor);

                _armor = newArmor;
                    
                OnArmorChanged?.Invoke(newArmor);
            } else if (item is Helmet newHelmet)
            {
                if (_helmet)
                    toDrop = Unequip(_helmet);
                
                _helmet = newHelmet;
                
                OnHelmetChanged?.Invoke(newHelmet);
            }

            return toDrop;
        }

        private Item Unequip(Item item)
        {
            if (item is Weapon)
            {
                _weapon = null;
                OnWeaponChanged?.Invoke(null);
            }
            else if (item is Armor)
            {
                _armor = null;
                OnArmorChanged?.Invoke(null);
            } else if (item is Helmet)
            {
                _helmet = null;
                OnHelmetChanged?.Invoke(null);
            }
            return item;
        }
    }
}
