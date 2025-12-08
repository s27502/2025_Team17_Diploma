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
        
        var hrgte = Instantiate(item.gameObject, _parent.transform);
        hrgte.transform.position = transform.position;
        
        item.SetPrice(_price);
        _price.GetComponent<TextMeshPro>().text = item.GetItemPrice().ToString();
        _price.SetActive(true);
    }
}
