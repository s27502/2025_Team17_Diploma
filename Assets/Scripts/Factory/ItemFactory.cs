using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Random = System.Random;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] protected List<Item> items = new List<Item>();
    [SerializeField] protected GameObject itemParent;

    private void Start()
    {
        SpawnItem();
    }

    protected virtual void SpawnItem()
    {
        var rnd = new Random();
        var item = items[rnd.Next(0, items.Count)];
        item.SetIsShop(false);
        var spawned = Instantiate(item.gameObject, itemParent.gameObject.transform);
        spawned.transform.position = transform.position;
        Destroy(gameObject);
    }
}
