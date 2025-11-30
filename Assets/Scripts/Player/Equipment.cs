using System;
using Items;
using UnityEngine;

namespace Player
{
    public class Equipment : MonoBehaviour
    {
        public event Action<Weapon> OnWeaponChanged;
        
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
            return item;
        }
    }
}
