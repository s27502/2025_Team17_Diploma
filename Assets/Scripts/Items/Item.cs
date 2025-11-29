using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private string _itemName;
    private int _itemPrice;
    private Sprite _sprite;
    //Stats should be in order: HP, MAX HP, DMG, ATTACK SPEED, LUCK, PROJECTILE COUNT;
    [SerializeField] private List<float> stats;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>().sprite;
    }

    public List<float> GetStats()
    {
        return stats;
    }

    public string GetItemName()
    {
        return _itemName;
    }

    public int GetItemPrice()
    {
        return _itemPrice;
    }

    public Sprite GetSprite()
    {
        return _sprite;
    }
}
