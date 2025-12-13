using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTP : MonoBehaviour
{
    private Room _room;
    private bool _hasTeleported = false;

    void Start()
    {
        _room = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasTeleported) return;

        if (other.CompareTag("Player"))
        {
            _hasTeleported = true;
            _room.GoToNextRoom();
        }
    }
}

