using UnityEngine;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        private Inventory _inventory;

        void Awake()
        {
            _inventory = GetComponent<Inventory>();
            ServiceLocator.Instance.Register(this);
        }
    }
}
