using System.Collections;
using System.Collections.Generic;
using Interactables;
using UnityEngine;

public class RoomReward : MonoBehaviour
{
    private CoinFactory _coinFactory;
    void Start()
    {
        _coinFactory = GetComponent<CoinFactory>();
    }

    public void SpawnReward()
    {
        var rnd = Random.Range(0, 100);
        if (rnd < 30)
        {
            //spawn halfHeart
        } else if (rnd < 60)
        {
            var amount = Random.Range(0, 10);
            _coinFactory.SpawnCoins(gameObject.transform.position, amount, gameObject);
        } else if (rnd < 80)
        {
            //spawn fullHeart
        }
    }
}
