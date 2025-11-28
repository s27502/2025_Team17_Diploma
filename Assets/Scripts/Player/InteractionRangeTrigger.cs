using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRangeTrigger : MonoBehaviour
{
    private PlayerInteractions _playerInteractions;

    private void Start()
    {
        _playerInteractions = GetComponentInParent<PlayerInteractions>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _playerInteractions?.OnRangeEnter(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _playerInteractions?.OnRangeExit(other);
    }
}
