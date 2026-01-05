using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _defDistanceRay = 100f;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _damage = 1;

    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _playerMask;

    private void Update()
    {
        ShootLaser();
    }

    private void ShootLaser()
    {
        Vector2 start = _firePoint.position;
        Vector2 dir = _firePoint.right;
        
        RaycastHit2D obstacleHit = Physics2D.Raycast(
            start,
            dir,
            _defDistanceRay,
            _obstacleMask
        );

        Vector2 endPoint = obstacleHit.collider != null
            ? obstacleHit.point
            : start + dir * _defDistanceRay;

        DrawLaser(start, endPoint);
        
        RaycastHit2D playerHit = Physics2D.Raycast(
            start,
            dir,
            Vector2.Distance(start, endPoint),
            _playerMask
        );

        if (playerHit.collider != null)
        {
            playerHit.collider
                .GetComponent<PlayerStats>()
                ?.ModifyHp(-_damage);
        }
    }

    private void DrawLaser(Vector2 start, Vector2 end)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }


}
