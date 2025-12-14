using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("spikes dealing dmg");
            other.GetComponent<PlayerStats>().ModifyHp(-1);
        }
    }
}
