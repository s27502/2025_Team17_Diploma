using UnityEngine;

public enum ScorpionAttack
{
    SnipingPlayer,
    ShootSwapping
}

namespace Enemies
{
    public class Scorpion : Enemy
    {
        [SerializeField] private float _attackChangeInterval = 3;
        private float _changeCounter = 0;

        private int _shootSwapState = 0;
        private float _shootCounter = 0f;
        private ScorpionAttack _currentAttack;

        protected override void OnStartAfterDelay()
        {
            base.OnStartAfterDelay();
            _currentAttack = RollAttack();
        }

        protected override void Attack()
        {
            if (_changeCounter >= _attackChangeInterval)
            {
                _currentAttack = RollAttack();
                _changeCounter = 0f;
                _shootSwapState = 0; 
                _shootCounter = 0f;
            }

            _changeCounter += Time.fixedDeltaTime;

            switch (_currentAttack)
            {
                case ScorpionAttack.SnipingPlayer:
                    SnipePlayer();
                    break;

                case ScorpionAttack.ShootSwapping:
                    ShootSwap();
                    break;
            }
        }

        private void Shoot4()
        {
            Vector2[] dirs =
            {
                Vector2.up,
                Vector2.down,
                Vector2.left,
                Vector2.right
            };

            foreach (var dir in dirs)
                projectileFactory.Shoot(dir);
        }

        private void ShootX()
        {
            Vector2[] dirs =
            {
                new Vector2(1, 1).normalized,
                new Vector2(-1, 1).normalized,
                new Vector2(1, -1).normalized,
                new Vector2(-1, -1).normalized
            };

            foreach (var dir in dirs)
                projectileFactory.Shoot(dir);
        }

        private void SnipePlayer()
        {
            if (_shootCounter <= 0f)
            {
                _shootCounter = EnemyStats.GetFireRate();

                Vector2 dir = (_player.transform.position - transform.position).normalized;
                projectileFactory.Shoot(dir);
            }

            _shootCounter -= Time.fixedDeltaTime;
        }

        private void ShootSwap()
        {
            if (_shootCounter <= 0f)
            {
                _shootCounter = EnemyStats.GetFireRate();

                if (_shootSwapState == 0)
                {
                    Shoot4();
                    _shootSwapState = 1;
                }
                else
                {
                    ShootX();
                    _shootSwapState = 0;
                }
            }

            _shootCounter -= Time.fixedDeltaTime;
        }

        private ScorpionAttack RollAttack()
        {
            return Random.value < 0.5f
                ? ScorpionAttack.SnipingPlayer
                : ScorpionAttack.ShootSwapping;
        }
    }
}
