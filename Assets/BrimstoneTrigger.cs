using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrimstoneTrigger : MonoBehaviour
{
    [SerializeField] private Enemy myEnemy;
    private HieraFollower _follower;
    
    [SerializeField] private bool shootRight;
    [SerializeField] private bool shootLeft;
    [SerializeField] private bool shootUp;
    [SerializeField] private bool shootDown;
    [SerializeField] private bool shootUpRight;
    [SerializeField] private bool shootUpLeft;
    [SerializeField] private bool shootDownRight;
    [SerializeField] private bool shootDownLeft;

    private MultiLaserSpawner.LaserDirection _myDirection;

    private void Awake()
    {
        _follower = myEnemy as HieraFollower;
        _myDirection = GetDir();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _follower.ActivateLaser(_myDirection);
        }
    }

    private MultiLaserSpawner.LaserDirection GetDir()
    {
        if (shootRight) return MultiLaserSpawner.LaserDirection.Right;
        if (shootLeft) return MultiLaserSpawner.LaserDirection.Left;
        if (shootUp) return MultiLaserSpawner.LaserDirection.Up;
        if (shootDown) return MultiLaserSpawner.LaserDirection.Down;
        if (shootUpRight) return MultiLaserSpawner.LaserDirection.UpRight;
        if (shootUpLeft) return MultiLaserSpawner.LaserDirection.UpLeft;
        if (shootDownRight) return MultiLaserSpawner.LaserDirection.DownRight;
        if (shootDownLeft) return MultiLaserSpawner.LaserDirection.DownLeft;
        return MultiLaserSpawner.LaserDirection.Right;
    }
}
