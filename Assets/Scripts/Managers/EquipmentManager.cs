using Player;
using UnityEngine;

namespace Managers
{
    public class EquipmentManager : MonoBehaviour
    {
        private Equipment _equipment;

        void Awake()
        {
            _equipment = GetComponent<Equipment>();
            Debug.Log(_equipment);
            ServiceLocator.Instance.Register(this);
        }

        public Equipment GetEquipment()
        {
            return _equipment;
        }
    }
}
