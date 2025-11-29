using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Inventory _inventory;

    void Awake()
    {
        _inventory = GetComponent<Inventory>();
        ServiceLocator.Instance.Register(this);
    }
}
