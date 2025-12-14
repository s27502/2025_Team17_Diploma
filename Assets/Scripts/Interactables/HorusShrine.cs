using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Interactables
{
    public class HorusShrine : MonoBehaviour, IInteractable
    {
        List<Blessing> _blessings;
        public void OnInteract()
        {
            Debug.Log("Interacted with horus shrine");
        }
    }
}
