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
    
    [SerializeField] private Sprite _startSprite;
    [SerializeField] private Sprite _endSprite;

    private SpriteRenderer _laserStart;
    private SpriteRenderer _laserEnd;

    private void Awake()
    {
        GameObject startObj = new GameObject("LaserStart");
        startObj.transform.parent = transform; 
        _laserStart = startObj.AddComponent<SpriteRenderer>();
        _laserStart.sprite = _startSprite;
        
        GameObject endObj = new GameObject("LaserEnd");
        endObj.transform.parent = transform;
        _laserEnd = endObj.AddComponent<SpriteRenderer>();
        _laserEnd.sprite = _endSprite;
    }


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
        
        if (_laserStart != null)
        {
            _laserStart.transform.position = start;
            Vector2 dirStart = end - start;
            _laserStart.transform.right = dirStart;
        }

        if (_laserEnd != null)
        {
            _laserEnd.transform.position = end;
            Vector2 dirEnd = end - start;
            _laserEnd.transform.right = dirEnd;
        }
        
        if (_lineRenderer != null)
        {
            float distance = Vector2.Distance(start, end);
            _lineRenderer.widthMultiplier = _laserStart != null ? _laserStart.bounds.size.x : 0.1f;
            if (_lineRenderer.material != null)
            {
                _lineRenderer.material.mainTextureScale = new Vector2(distance, 1);
            }
        }
    }



}
