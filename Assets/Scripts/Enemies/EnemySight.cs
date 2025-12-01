using System;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    protected Enemy Enemy;
   

    protected virtual void Start()
    {
        Enemy = gameObject.GetComponentInParent<Enemy>();
    }
    

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy.SetPlayer(other.gameObject);
            Debug.Log("player found");
            Enemy.StartAttacking();
        }
    }
    
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy.SetPlayer(null);
            Enemy.StartAttacking();
        }
    }
    
    
    
}
