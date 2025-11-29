using UnityEngine;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        private Inventory _inventory;
        private GameObject _itemStorage;
        void Awake()
        {
            _inventory = GetComponent<Inventory>();
            CreateItemStorage();
            ServiceLocator.Instance.Register(this);
        }

        private void CreateItemStorage()
        {
            _itemStorage = new GameObject("ItemStorage");
            _itemStorage.transform.SetParent(transform);
        }

        public GameObject GetItemStorage()
        {
            return _itemStorage;
        }
        public Inventory GetInventory()
        {
            return _inventory;
        }
    }
}
