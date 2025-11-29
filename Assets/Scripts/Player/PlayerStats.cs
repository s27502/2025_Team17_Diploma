using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int maxHp;
    [SerializeField] private int dmg;
    [SerializeField] private int atkSpeed;
    [SerializeField] private int luck;
    [SerializeField] private int projectileCount;
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
    
    public void ModifyDmg(int value)
    {
        dmg += value;
    }
    
    public void ModifyAtkSpeed(int value)
    {
        //Attack Speed capped
        if (atkSpeed + value > 10)
        {
            atkSpeed = 10;
        }
        else
        {
            atkSpeed += value;
        }
    }
    
    public void ModifyCoins(int value)
    {
        coins += value;
    }
    
    public void ModifyProjectileCount(int value)
    {
        projectileCount += value;
    }
    
    public void ModifyLuck(int value)
    {
        luck += value;
    }
    
    public int GetProjectileCount()
    {
        return projectileCount;
    }
    
    public int GetLuck()
    {
        return luck;
    }
    
    public int GetHp()
    {
        return hp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public int GetDmg()
    {
        return dmg;
    }

    public int GetAtkSpeed()
    {
        return atkSpeed;
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
