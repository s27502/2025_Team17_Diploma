using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Random = System.Random;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] protected List<Item> items = new List<Item>();
    private void Awake()
    {
        SpawnItem();
    }


    protected virtual void SpawnItem()
    {
        var rnd = new Random();
        var item = items[rnd.Next(0, items.Count)];
        item.SetIsShop(false);
        Instantiate(item.gameObject, gameObject.transform.position, Quaternion.identity);
        item.transform.SetParent(gameObject.transform.parent);
        Destroy(gameObject);
    }
}
