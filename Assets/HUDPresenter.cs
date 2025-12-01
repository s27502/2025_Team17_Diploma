using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class HUDPresenter : MonoBehaviour
{
    private HUDView _view;
    private PlayerStats _playerStats;
    private Equipment _equipment;
    private void Awake()
    {
        _view = GetComponent<HUDView>();
    }

    private void Start()
    {
        _playerStats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
        _equipment = ServiceLocator.Instance.GetService<EquipmentManager>().GetEquipment();

        _equipment.OnWeaponChanged += HandleWeaponChange;
        
        _playerStats.onDamageChanged.AddListener(HandleDamageChange);
        _playerStats.onAtkSpdChanged.AddListener(HandleAttackSpeedChange);
        _playerStats.onProjCountChanged.AddListener(HandleProjectileCountChange);
        _playerStats.onLuckChanged.AddListener(HandleLuckChange);
        _playerStats.onCoinsChanged.AddListener(HandleCoinChange);
        SetStats();
    }

    private void SetStats()
    {
        _view.SetDamage(_playerStats.GetDmg());
        _view.SetAttackSpeed(_playerStats.GetAtkSpeed());
        _view.SetProjectileCount(_playerStats.GetProjectileCount());
        _view.SetLuck(_playerStats.GetLuck());
        _view.SetCoins(_playerStats.GetCoins());
    }
    
    private void HandleDamageChange(int value)
    {
        _view.SetDamage(value);
    }

    private void HandleAttackSpeedChange(float value)
    {
        _view.SetAttackSpeed(value);
    }

    private void HandleProjectileCountChange(int value)
    {
        _view.SetProjectileCount(value);
    }

    private void HandleLuckChange(int value)
    {
        _view.SetLuck(value);
    }

    private void HandleCoinChange(int value)
    {
        _view.SetCoins(value);
    }

    private void HandleWeaponChange(Weapon weapon)
    {
        var sprite = weapon != null ? weapon.GetSprite() : null;
        _view.SetWeapon(sprite);
    }
}
