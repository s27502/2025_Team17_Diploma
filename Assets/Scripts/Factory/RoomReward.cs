using System.Collections;
using System.Collections.Generic;
using Interactables;
using Managers;
using Player;
using UnityEngine;

public class RoomReward : MonoBehaviour
{
    private CoinFactory _coinFactory;
    private PlayerStats _playerStats;
    [SerializeField] private GameObject _halfHeart;
    [SerializeField] private GameObject _heart;
    void Start()
    {
        _coinFactory = GetComponent<CoinFactory>();
        _playerStats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
        SpawnReward();
    }

    public void SpawnReward()
    {
        var rnd = Random.Range(0, 100);
        if (rnd < 41)
        {
            GameObject v;
            rnd = Random.Range(0, 100);
            rnd += _playerStats.GetLuck();
            if(rnd < 51)
            {
                v = Instantiate(_halfHeart, transform.position, Quaternion.identity);
                v.transform.SetParent(gameObject.transform);
            }
            else
            {
                v = Instantiate(_heart, transform.position, Quaternion.identity);
                v.transform.SetParent(gameObject.transform);
            }
        } else 
        {
            rnd = Random.Range(1, 15);
            rnd += _playerStats.GetLuck();
            _coinFactory.SpawnCoins(gameObject.transform.position, rnd, gameObject);
        } 
    }
}
