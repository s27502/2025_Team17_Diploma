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
    public void ModifyHp(int value)
    {
        hp += value;
        if (hp < 0)
        {
            hp = 0;
            _dead.Invoke();
        }
    }
    
    public void ModifyMaxHp(int value)
    {
        if (maxHp + value > 0)
        {
            maxHp += value;
        }
    }
    
    public void ModifyStrength(int value)
    {
        strength += value;
    }
    
    public void ModifyAgility(int value)
    {
        agility += value;
    }
    
    public void ModifyCoins(int value)
    {
        coins += value;
    }

    public int GetHp()
    {
        return hp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public int GetStrength()
    {
        return strength;
    }

    public int GetAgility()
    {
        return agility;
    }

    public int GetCoins()
    {
        return coins;
    }
    
    void OnEventTriggered()
    {
        Debug.Log("Player Died");
    }
}
