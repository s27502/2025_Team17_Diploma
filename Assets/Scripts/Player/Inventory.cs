using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> _items = new List<Item>();

    public List<Item> GetItems()
    {
        return _items;
    }

    public void AddItem(Item item)
    {
        _items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        _items.Remove(item);
    }
}
