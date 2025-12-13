using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    private SpriteRenderer _sr;
    private PlayerStats _stats;
    
    [SerializeField] private Sprite full;
    [SerializeField] private Sprite half;

    [SerializeField] private int healAmount;
    void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
        _stats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
        if (healAmount == 1)
        {
            _sr.sprite = half;
        } else if (healAmount == 2)
        {
            _sr.sprite = full;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_stats.GetMaxHp() > _stats.GetHp())
            {
                _stats.ModifyHp(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
