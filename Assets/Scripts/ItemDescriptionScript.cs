using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Managers;
using Player;
using TMPro;
using UnityEngine;

public class ItemDescriptionScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    
    private PlayerStats _playerStats;

    private void Awake()
    {
        ServiceLocator.Instance.Register(this);
        SetClear();
    }

    private void Start()
    {
        _playerStats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
        gameObject.SetActive(false);
    }
    
    public void SetDescription(Item item)
    {
        _name.SetText(item.GetItemName());
        _description.SetText(item.GetDescription());
    }

    public void SetClear()
    {
        _name.SetText("");
        _description.SetText("");
    }
}
