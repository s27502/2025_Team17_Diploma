using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public enum AnubisAttacks
{
    SpikeSpawn
}
public class Anubis : Boss
{
    [SerializeField] private List<GameObject> cornerPositions = new List<GameObject>();
    private List<Vector2> _cornerPositions = new List<Vector2>();
    [SerializeField] private List<GameObject> spikeObjects;
    [SerializeField] private float spikeLifetime = 2f;
    private float _spikeLifetimeCounter = 0f;
    private bool _delaySpikes = false;
    [SerializeField] private float delaySpikesTime = 1f;
    private float _delaySpikesCounter = 0f;

    private float _attackDelayCounter;

    private int _currentCorner;
    private bool _goToCorner = false;

    private bool _rollNewAttack = true;
    private AnubisAttacks _currentAttack;

    private void Awake()
    {
        InitiateCornerList();
    }

    protected override void Attack()
    {
        if (_rollNewAttack)
        {
            SetUpSpikeAttack();
            _currentAttack = AnubisAttacks.SpikeSpawn;

        }
        else
        {
            switch (_currentAttack)
            {
                case AnubisAttacks.SpikeSpawn:
                    PerformSpikeSpawn();
                    break;
            }
        }
    }

    private void PerformSpikeSpawn()
    {
        if (_goToCorner)
        {
            GoToCorner();
        }
        else if (_delaySpikes)
        {
            if (_delaySpikesCounter <= delaySpikesTime)
            {
                _delaySpikesCounter += Time.fixedDeltaTime;
            }
            else
            {
                _delaySpikes = false;
                spikeObjects[_currentCorner].SetActive(true);
            }
        }
        else
        {
            if (_spikeLifetimeCounter <= spikeLifetime)
            {
                _spikeLifetimeCounter += Time.fixedDeltaTime;
            }
            else
            {
                spikeObjects[_currentCorner].SetActive(false);
                DelayNextAttack();
            }
        }
    }

    private void GoToCorner()
    {
        float distance = Vector2.Distance(transform.position, _cornerPositions[_currentCorner]);
        Debug.Log(distance);
        
        if (distance > 0.1f)
        {
            MoveTo(_cornerPositions[_currentCorner]);
        }
        else
        {
            _goToCorner = false;
            _delaySpikes = true;
        }
    }

    private void InitiateCornerList()
    {
        foreach (GameObject cornerPos in cornerPositions)
        {
            _cornerPositions.Add(cornerPos.transform.position);
        }
    }

    private int GetFurthestCornerIndex()
    {
        int furthestIndex = 0;
        float maxDistance = float.MinValue;

        Vector2 anubisPos = transform.position;

        for (int i = 0; i < _cornerPositions.Count; i++)
        {
            float distance = Vector2.Distance(anubisPos, _cornerPositions[i]);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestIndex = i;
            }
        }

        return furthestIndex;
    }

    private void SetUpSpikeAttack()
    {
        _goToCorner = true;
        _currentCorner = GetFurthestCornerIndex();
        _rollNewAttack = false;
        _delaySpikesCounter = 0f;
        _spikeLifetimeCounter = 0f;
    }
    
    private void DelayNextAttack()
    {
        if (_attackDelayCounter <= EnemyStats.GetAtkSpd())
        {
            _attackDelayCounter += Time.fixedDeltaTime;
        }
        else
        {
            _rollNewAttack = true;
            _attackDelayCounter = 0f;
            //_agent.enabled = true;
        }
    }

}
