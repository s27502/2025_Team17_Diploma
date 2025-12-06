using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Managers;
using Player;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private SpriteFlipper  _spriteFlipper;
    private Equipment _equipment;
    void Awake()
    {
        ServiceLocator.Instance.Register(this);
        
    }

    private void Start()
    {
        _spriteFlipper = GetComponent<SpriteFlipper>();
        _equipment = ServiceLocator.Instance.GetService<EquipmentManager>().GetEquipment();
        _equipment.OnArmorChanged +=SwitchArmor;
        _equipment.OnHelmetChanged +=SwitchHelmet;
    }

    private void SwitchHelmet(Helmet helmet)
    {
        Debug.Log("Switching Helmet");
        _spriteFlipper.GetHelmetRenderer().enabled = true;
        
    }

    private void SwitchArmor(Armor armor)
    {
        Debug.Log("Switching Armor");
        _spriteFlipper.GetArmorRenderer().enabled = true;
        
        
    }
    
    public SpriteFlipper GetSpriteFlipper()
    {
        return _spriteFlipper;
    }
}
