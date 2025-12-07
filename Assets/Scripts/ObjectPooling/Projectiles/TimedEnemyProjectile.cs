using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class TimedEnemyProjectile : EnemyProjectile
{
    [SerializeField] private float _lifeTime = 5f;
    private float _lifeCounter;

    protected override void Awake()
    {
        base.Awake();
        _lifeCounter = _lifeTime;
    }

    private void FixedUpdate()
    {
        if (_lifeCounter <= 0)
        {
            _pool?.ReleaseObject(this);
        }

        _lifeCounter -= Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>()?.ModifyHp(-damage);
            _pool?.ReleaseObject(this);
        }
    }
}
