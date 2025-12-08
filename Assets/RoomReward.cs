using System.Collections;
using System.Collections.Generic;
using Interactables;
using UnityEngine;

public class RoomReward : MonoBehaviour
{
    private CoinFactory _coinFactory;
    [SerializeField] private GameObject _halfHeart;
    [SerializeField] private GameObject _heart;
    void Start()
    {
        _coinFactory = GetComponent<CoinFactory>();
        SpawnReward();
    }

    public void SpawnReward()
    {
        var rnd = Random.Range(0, 100);
        if (rnd < 30)
        {
            GameObject v = Instantiate(_heart, transform.position, Quaternion.identity);
            v.transform.SetParent(gameObject.transform);
        } else if (rnd < 60)
        {
            var amount = Random.Range(0, 10);
            _coinFactory.SpawnCoins(gameObject.transform.position, amount, gameObject);
        } else if (rnd < 100)
        {
            GameObject v = Instantiate(_halfHeart, transform.position, Quaternion.identity);
            v.transform.SetParent(gameObject.transform);
        }
    }
}
