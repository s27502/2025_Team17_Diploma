using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private IInteractable _interactable;

    public void Interact()
    {
        if (_interactable != null)
        {
            _interactable.OnInteract();
        }
    }

    public void OnRangeEnter(Collider2D other)
    {
        _interactable = other.GetComponent<IInteractable>();
    }

    public void OnRangeExit(Collider2D other)
    {
        _interactable = null;
    }
}
