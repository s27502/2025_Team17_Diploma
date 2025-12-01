using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Room : MonoBehaviour
{
    private FloorManager _floorManager;
    [SerializeField] private GameObject _playerTP;
    [SerializeField] private GameObject _enemies;

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
        }
    }

    public void GoToNextRoom()
    {
        _floorManager.GoToNextRoom();
    }
}
