using System;
using Enemies;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    protected Enemy Enemy;
   
    protected virtual void Start()
    {
        Enemy = GetComponentInParent<Enemy>();
    }

    private void Awake()
    {
        Enemy = GetComponentInParent<Enemy>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        Enemy.SetPlayer(other.gameObject);

        if (Enemy.CanReact())
            Enemy.StartAttacking();
    }
    
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Enemy.SetPlayer(null);
        Enemy.StopAttacking();
    }
}

