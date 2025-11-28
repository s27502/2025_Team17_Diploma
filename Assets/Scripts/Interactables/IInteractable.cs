using UnityEngine;

namespace DefaultNamespace
{
    public interface IInteractable
    {
        public void OnInteract(){}
        public void OnInteract(GameObject player){}
    }
}