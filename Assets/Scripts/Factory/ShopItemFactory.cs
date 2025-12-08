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
        Item item = items[i];
        item.SetIsShop(true);
        
        Instantiate(item.gameObject, empty.transform.position, empty.transform.rotation);
        Destroy(empty);
        
        item.transform.position = transform.position;
        item.transform.SetParent(gameObject.transform.parent);
        item.SetPrice(_price);
        _price.GetComponent<TextMeshPro>().text = item.GetItemPrice().ToString();
        _price.SetActive(true);
        Destroy(gameObject);
    }
}
