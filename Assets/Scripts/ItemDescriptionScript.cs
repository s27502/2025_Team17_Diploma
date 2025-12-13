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
    [SerializeField] private List<TextMeshProUGUI> _stats;
    [SerializeField] private List<HPSlot> _arrows;
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

    public void SetName(string name)
    {
        _name.text = name;
    }

    public void SetDescription(Item item)
    {
        _name.SetText(item.GetItemName());
        foreach (var stat in item.GetStats())
        {
            if (item is Weapon)
            {
                _stats[0].SetText("Damage: " + item.GetStats()[0]);
                _stats[1].SetText("Fire rate: " + item.GetStats()[1]);
            }
            else if (item is Armor)
            {
                _stats[0].SetText("Armor: " + item.GetStats()[0]);
            } else if (item is Helmet)
            {
                _stats[0].SetText("Armor: " + item.GetStats()[0]);
            }
            else
            {
                if (item.GetStats()[0] > 0)
                {
                    _stats[0].SetText("Damage: " + item.GetStats()[0]);
                }
                if (item.GetStats()[1] > 0)
                {
                    _stats[0].SetText("Fire rate: " + item.GetStats()[1]);
                }
                if (item.GetStats()[2] > 0)
                {
                    _stats[0].SetText("Luck: " + item.GetStats()[2]);
                }
                if (item.GetStats()[3] > 0)
                {
                    _stats[0].SetText("HP: " + item.GetStats()[3]);
                }
            }
        }
    }

    public void SetClear()
    {
        _name.SetText("");
        foreach (var stat in _stats)
        {
            stat.SetText("");
        }
    }
}
