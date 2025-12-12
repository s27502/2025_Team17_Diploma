using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Enemies
{
    public class Snake : Enemy
    {
        private bool _isNotDashing = true;
        [SerializeField] private float _dashSpeedMult = 7f;

        private float _shootCounter;
        

        private float _posChangeInterval = 0.5f;
        private float _posChangeCounter = 0f;

        private Vector2 _currentRandomTarget;
        
        private float _randomMoveDistance = 2f;
        private bool _obstacleFound = false;

        protected override void OnStartAfterDelay()
        {
            base.OnStartAfterDelay();
            StartAttacking();
        }

        protected override void Attack()
        {
            if (_isNotDashing)
            {
                MoveToRandomDirection();
                Shoot8();
            }

            base.Attack();
        }

        private void MoveToRandomDirection()
        {
            _posChangeCounter -= Time.fixedDeltaTime;
            
            if (_posChangeCounter <= 0f)
            {
                PickNewRandomTarget360();
                _posChangeCounter = _posChangeInterval;
            }
            
            MoveTo(_currentRandomTarget);
        }

        private void PickNewRandomTarget360()
        {
            float angle = Random.Range(0f, 360f);
            
            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ).normalized;
            
            _currentRandomTarget = (Vector2)_rb.position + dir * _randomMoveDistance;
        }

        public void StartDashing(Vector3 targetPos)
        {
            if (!_isNotDashing) return;

            _isNotDashing = false;
            StartCoroutine(DashCoroutine(targetPos));
        }

        private void Shoot8()
        {
            if (_shootCounter <= 0f)
            {
                _shootCounter = EnemyStats.GetFireRate();

                Vector2[] dirs =
                {
                    Vector2.up,
                    Vector2.down,
                    Vector2.left,
                    Vector2.right,
                    new Vector2(1, 1).normalized,
                    new Vector2(-1, 1).normalized,
                    new Vector2(1, -1).normalized,
                    new Vector2(-1, -1).normalized
                };

                foreach (var dir in dirs)
                {
                    projectileFactory.Shoot(dir);
                }
            }

            _shootCounter -= Time.fixedDeltaTime;
        }


        private IEnumerator DashCoroutine(Vector3 targetPos)
        {
            Debug.Log("Dash start");
            EnemyStats.SetMovementSpeed(EnemyStats.GetMovementSpeed() * _dashSpeedMult);
            
            while (Vector2.Distance(_rb.position, targetPos) > 0.1f && !_obstacleFound)
            {
                MoveTo(targetPos);
                yield return new WaitForFixedUpdate();
            }

            
            EnemyStats.SetMovementSpeed(EnemyStats.GetMovementSpeed() / _dashSpeedMult);
            _obstacleFound = false;
            _isNotDashing = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Player")) && !_isNotDashing)
            {
                _obstacleFound = true;
            }
        }

    }
}