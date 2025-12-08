using System.Collections;
using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    private SpriteRenderer _sr;
    private PlayerStats _stats;
    
    [SerializeField] private Sprite full;
    [SerializeField] private Sprite empty;

    [SerializeField] private int healAmount;
    void Start()
    {
        _stats = ServiceLocator.Instance.GetService<PlayerStatManager>().GetPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
