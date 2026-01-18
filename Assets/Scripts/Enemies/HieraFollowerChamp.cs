using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class HieraFollowerChamp : HieraFollower
    {
        private bool _notShooting = true;

        public void ActivateLaser()
        {
            StartCoroutine(ActivateAllLaserCoroutine());
        }

        private IEnumerator ActivateAllLaserCoroutine()
        {
            if (_notShooting)
            {
                _notShooting = false;
                yield return new WaitForSeconds(WindUpTime);
                MultiLaserSpawner.SetActive(true);
                yield return new WaitForSeconds(LaserTime);
                MultiLaserSpawner.SetActive(false);
                _notShooting = true;
            }
        }
    }
}