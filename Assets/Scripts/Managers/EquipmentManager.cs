using Player;
using UnityEngine;

namespace Managers
{
    public class EquipmentManager : MonoBehaviour
    {
        private Equipment _equipment;
        private SpriteFlipper _flipper;

        void Awake()
        {
            _equipment = GetComponent<Equipment>();
            
            ServiceLocator.Instance.Register(this);
        }

        public Equipment GetEquipment()
        {
            return _equipment;
        }
    }
}
