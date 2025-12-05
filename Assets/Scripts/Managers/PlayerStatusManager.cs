using System;
using StatusSystem;
using UnityEngine;

namespace Managers
{
    public class PlayerStatusManager : MonoBehaviour
    {
        private StatusHandler _playerStatusHandler;

        private void Awake()
        {
            _playerStatusHandler = GetComponent<StatusHandler>();
            ServiceLocator.Instance.Register(this);
        }

        public StatusHandler GetPlayerStatusHandler()
        {
            return _playerStatusHandler;
        }
    }
}