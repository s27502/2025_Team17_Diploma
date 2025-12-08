using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Room : MonoBehaviour
{
    private FloorManager _floorManager;
    [SerializeField] private bool _giveReward = false;
    [SerializeField] private GameObject _playerTP;
    [SerializeField] private GameObject _enemies;
    [SerializeField] private GameObject _doorClosed;
    [SerializeField] private GameObject _doorOpen;
    [SerializeField] private GameObject _roomReward;

    public GameObject _spawnPos;

    void Start()
    {
        _floorManager = ServiceLocator.Instance.GetService<FloorManager>();
    }

    private void FixedUpdate()
    {
        if (_enemies.transform.childCount == 0)
        {
            _playerTP.SetActive(true);
            _doorClosed.SetActive(false);
            _doorOpen.SetActive(true);
            if (_giveReward)
            {
                _roomReward.SetActive(true);
            }
        }
    }

    public void GoToNextRoom()
    {
        _floorManager.GoToNextRoom();
    }
}
