using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTP : MonoBehaviour
{
    private Room _room;
    void Start()
    {
        _room = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _room.GoToNextRoom();
        }
    }
}
