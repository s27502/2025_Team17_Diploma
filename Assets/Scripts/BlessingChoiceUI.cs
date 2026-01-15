using System;
using System.Collections;
using System.Collections.Generic;
using Interactables;
using Managers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

public class BlessingChoiceUI : MonoBehaviour
{
    private Blessing _blessing1;
    private Blessing _blessing2;
    private Blessing _choice;
    private PlayerStats _playerStats;
    
    [SerializeField] private TMP_Text _btn1;
    [SerializeField] private TMP_Text _btn2;
    private void Start()
    {
        _playerStats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        ApplyStats();
        gameObject.SetActive(false);
    }

    public void SetBlessings(Blessing blessing1, Blessing blessing2)
    {
        _blessing1 = blessing1;
        _btn1.text = blessing1.GetDescription();
        _blessing2 = blessing2;
        _btn2.text = blessing2.GetDescription();
    }
    
    public void SetChoice1()
    {
        _choice = _blessing1;
        HideUI();
    }
    
    public void SetChoice2()
    {
        _choice = _blessing2;
        HideUI();
    }
    
    private void ApplyStats()
    {
        if (_choice != null)
        {
            if (_choice.GetAtkSpdMult() > 0)
            {
                var p = _playerStats.GetAtkSpeed() * _choice.GetAtkSpdMult();
                
                _playerStats.SetAtkSpd(p);
                _playerStats.ModifyDmg((int)(_playerStats.GetDmg()*_choice.GetDmgMult()));
            }

            if (_choice.GetDmgMult() > 0)
            {
                _playerStats.SetDamage((int)(_playerStats.GetDmg()*_choice.GetDmgMult()));
                _playerStats.ModifyAttackSpeed(_playerStats.GetAtkSpeed() * _choice.GetAtkSpdMult());
            }

            if (_choice.GetAtkSpdMult() != 0)
            {
                _playerStats.ModifyAttackSpeed(_playerStats.GetAtkSpeed() * _choice.GetAtkSpdMult());
            }

            if (_choice.GetLuck() > 0)
            {
                _playerStats.ModifyLuck(_choice.GetLuck());
                _playerStats.ModifyCoins(-_playerStats.GetCoins());
            }

            if (_choice.GetHpCap() > 0)
            {
                _playerStats.CapHp(_choice.GetHpCap());
                _playerStats.SetDamage((int)(_playerStats.GetDmg()*_choice.GetDmgMult()));
            }
            _playerStats.ModifyMaxHp(_choice.GetHp());
            _playerStats.ModifyHp(0);
            _playerStats.ModifyCoins(_choice.GetCoins());
            _playerStats.ModifyProjectileCount(_choice.GetProjCount());
        }
    }
    
}
