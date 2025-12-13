using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Items;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private IInteractable _interactable;
    private ItemDescriptionScript _description;

    private void Start()
    {
        _description = ServiceLocator.Instance.GetService<ItemDescriptionScript>();
    }

    public void Interact()
    {
        if (_interactable != null)
        {
            _interactable.OnInteract();
        }
    }

    public void OnRangeEnter(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            _interactable = interactable;
        if (interactable is Item)
        {
            _description.gameObject.SetActive(true);
            _description.SetDescription(interactable as Item);
        }
            
    }

    
    public void OnRangeExit(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null && _interactable == interactable)
            _interactable = null;
        if (_description)
        {
            _description.gameObject.SetActive(false);
            _description.SetClear();
        }
    }

}
