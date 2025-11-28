using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int maxHp;
    [SerializeField] private int strength;
    [SerializeField] private int agility;
    [SerializeField] private int coins;

    private UnityEvent _dead;

    void Start()
    {
        if (_dead == null)
            _dead = new UnityEvent();

        _dead.AddListener(OnEventTriggered);
    }
    
    //negative values to subtract
    void ModifyHp(int value)
    {
        hp += value;
        if (hp < 0)
        {
            hp = 0;
            _dead.Invoke();
        }
    }
    
    void ModifyMaxHp(int value)
    {
        if (maxHp + value > 0)
        {
            maxHp += value;
        }
    }
    
    void ModifyStrength(int value)
    {
        strength += value;
    }
    
    void ModifyAgility(int value)
    {
        agility += value;
    }
    
    void ModifyCoins(int value)
    {
        coins += value;
    }

    int GetHp()
    {
        return hp;
    }

    int GetMaxHp()
    {
        return maxHp;
    }

    int GetStrength()
    {
        return strength;
    }

    int GetAgility()
    {
        return agility;
    }

    int GetCoins()
    {
        return coins;
    }
    
    void OnEventTriggered()
    {
        Debug.Log("Player Died");
    }
}
