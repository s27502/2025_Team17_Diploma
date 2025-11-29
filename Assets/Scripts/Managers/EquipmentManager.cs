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
            ServiceLocator.Instance.Register(this);
        }
    }
}
