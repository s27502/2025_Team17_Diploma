using Player;
using UnityEngine;

namespace Managers
{
    public class PlayerStatManager : MonoBehaviour
    {
        private PlayerStats _playerStats;

        void Awake()
        {
            _playerStats = GetComponent<PlayerStats>();
            ServiceLocator.Instance.Register(this);
        }

        public PlayerStats GetPlayerStats()
        {
            return _playerStats;
        }
    }
}
