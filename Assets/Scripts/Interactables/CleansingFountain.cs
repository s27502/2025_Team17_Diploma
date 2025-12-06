using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Managers;
using StatusSystem;
using UnityEngine;

public class CleansingFountain : MonoBehaviour, IInteractable
{
    private StatusHandler _playerStatusHandler;
    private void Awake()
    {
        _playerStatusHandler = ServiceLocator.Instance.GetService<PlayerStatusManager>().GetPlayerStatusHandler();
    }

    public void OnInteract()
    {
        _playerStatusHandler.Cleanse();
    }
}
