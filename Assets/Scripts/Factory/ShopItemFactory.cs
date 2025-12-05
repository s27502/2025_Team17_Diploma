using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Random = System.Random;

public class ShopItemFactory : ItemFactory
{
    protected override void SpawnItem()
    {
        var rnd = new Random();
        var i = rnd.Next(0, items.Count);
        Item item = items[i];
        item.SetIsShop(true);
        Instantiate(item.gameObject, gameObject.transform.position, Quaternion.identity);
    }
}
