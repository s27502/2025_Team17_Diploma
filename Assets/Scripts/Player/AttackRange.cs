using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackRange : MonoBehaviour
{
    private PlayerAttack _playerAttack;

    private void Start()
    {
        _playerAttack = GetComponentInParent<PlayerAttack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!_playerAttack.EnemiesInRange.Contains(collision.gameObject))
            {
                _playerAttack.EnemiesInRange.Add(collision.gameObject);
                Debug.Log("Enemy entered range: " + collision.name);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (_playerAttack.EnemiesInRange.Contains(collision.gameObject))
            {
                _playerAttack.EnemiesInRange.Remove(collision.gameObject);
                Debug.Log("Enemy exited range: " + collision.name);
            }
        }
    }
}