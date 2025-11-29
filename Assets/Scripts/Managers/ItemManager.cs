using System.Collections;
using System.Collections.Generic;
using Items;
using Managers;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private InventoryManager _inventoryManager;
    void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }

    void Start()
    {
        _inventoryManager = ServiceLocator.Instance.GetService<InventoryManager>();
        Debug.Log(_inventoryManager);
    }

    public void PutInStorage(Item item)
    {
        item.transform.SetParent(_inventoryManager.GetItemStorage().transform);
        item.gameObject.SetActive(false);
    }
    
}
