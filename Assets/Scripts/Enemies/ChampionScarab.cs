using System;
using UnityEngine;

namespace Enemies
{
    public class ChampionScarab : Scarab
    {
        [SerializeField] private float _shootDelay = 1f;
        private float _shootTimer;
        protected override void Attack()
        {
            base.Attack();
            
            _shootTimer += Time.fixedDeltaTime;

            if (_shootTimer >= _shootDelay)
            {
                _shootTimer = 0f;
                Shoot();
            }
        }

        private void Shoot()
        {
            
        }
    }
}