using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Managers;
using Player;
using UnityEditor.Animations;
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
        if (helmet)
        {
            _spriteFlipper.GetHelmetRenderer().enabled = true;
            _spriteFlipper.SetHelmetAnimator(helmet.GetAnimator());
        }
    }

    private void SwitchArmor(Armor armor)
    {
        if (armor)
        {
            _spriteFlipper.GetArmorRenderer().enabled = true;
            _spriteFlipper.SetArmorAnimator(armor.GetAnimator());
        }
    }
    
    public SpriteFlipper GetSpriteFlipper()
    {
        return _spriteFlipper;
    }
    
}
