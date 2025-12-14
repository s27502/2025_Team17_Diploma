using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private GameObject _player;
    private void FixedUpdate()
    {
        if (_player != null)
        {
            _player.GetComponent<PlayerStats>().ModifyHp(-1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player = null;
        }
    }
}
