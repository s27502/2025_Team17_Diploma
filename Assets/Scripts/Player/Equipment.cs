using Items;
using UnityEngine;

namespace Player
{
    public class Equipment : MonoBehaviour
    {
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
            if (item.GetType() == typeof(Weapon))
            {
                if (_weapon)
                {
                    toDrop = Unequip(_weapon);
                }
                _weapon = item;
            }
            Debug.Log("Equipped " + item.name);
            return toDrop;
        }

        public Item Unequip(Item item)
        {
            if (item.GetType() == typeof(Weapon))
            {
                _weapon = null;
            }
            return item;
        }
    }
}
