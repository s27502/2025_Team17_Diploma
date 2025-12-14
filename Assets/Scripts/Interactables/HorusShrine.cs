using DefaultNamespace;
using UnityEngine;

namespace Interactables
{
    public class HorusShrine : MonoBehaviour, IInteractable
    {
        public void OnInteract()
        {
            Debug.Log("Interacted with horus shrine");
        }
    }
}
