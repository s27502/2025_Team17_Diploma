using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Random = System.Random;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] protected List<Item> items = new List<Item>();
    [SerializeField] protected GameObject _parent;
    private void Awake()
    {
        SpawnItem();
    }


    protected virtual void SpawnItem()
    {
        var rnd = new Random();
        var item = items[rnd.Next(0, items.Count)];
        item.SetIsShop(false);
        Debug.Log(item);
        var trhe = Instantiate(item.gameObject, _parent.transform);
        Debug.Log(trhe);
        trhe.transform.position = transform.position;
        
    }
}
