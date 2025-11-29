using System.Collections;
using System.Collections.Generic;
using Items;
using Managers;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private InventoryManager _inventoryManager;
    private FloorManager _floorManager;
    void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }

    void Start()
    {
        _inventoryManager = ServiceLocator.Instance.GetService<InventoryManager>();
        _floorManager = ServiceLocator.Instance.GetService<FloorManager>();
    }

    public void PutInStorage(Item item)
    {
        item.transform.SetParent(_inventoryManager.GetItemStorage().transform);
        item.gameObject.SetActive(false);
    }

    public void PutInRoom(Item item, Vector3 position)
    {
        item.transform.SetParent(_floorManager.GetCurrentRoom().transform);
        item.transform.position = position;
        item.gameObject.SetActive(true);
    }
}
