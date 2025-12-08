using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class ShopItemFactory : ItemFactory
{
    [SerializeField] private GameObject _price;
    [SerializeField] private GameObject empty;
    
    protected override void SpawnItem()
    {
        var rnd = new Random();
        var i = rnd.Next(0, items.Count);
        
        Item itemPrefab = items[i];
        itemPrefab.SetIsShop(true);
        
        var hrgte = Instantiate(itemPrefab.gameObject, _parent.transform);
        hrgte.transform.position = transform.position;

        var instanceItem = hrgte.GetComponent<Item>();
        instanceItem.SetPrice(_price);

        _price.GetComponent<TextMeshPro>().text = instanceItem.GetItemPrice().ToString();
        _price.SetActive(true);
    }

}
