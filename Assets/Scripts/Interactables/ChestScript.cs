using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ChestScript : MonoBehaviour, IInteractable
{
    [SerializeField] private int CoinAmount;
    
    private bool opened = false;
    private CoinFactory _coinFactory;
    
    private void Awake()
    {
        _coinFactory = GetComponent<CoinFactory>();
    }

    public void OnInteract()
    {
        if (opened)
        {
            return;
        }
        opened = true;
        _coinFactory.SpawnCoins(transform.position, CoinAmount);
    }
}
