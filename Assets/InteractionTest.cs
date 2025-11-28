using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class InteractionTest : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        Debug.Log("Interakcja");
    }
}
