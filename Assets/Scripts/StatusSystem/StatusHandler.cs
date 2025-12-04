using StatSystem;
using UnityEngine;

namespace StatusSystem
{
    public class StatusHandler : MonoBehaviour
    {
        [SerializeField] private Stats _stats;
        private bool _poisoned;
        private int _poisonDmg;
    }
}