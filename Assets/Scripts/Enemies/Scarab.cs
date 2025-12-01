using UnityEngine;

namespace Enemies
{
    public class Scarab : Enemy
    {
        [SerializeField] private float _dashDelay = 0.5f;
        [SerializeField] private float _dashDistance = 0.5f;

        private float _dashTimer;

        protected override void Attack()
        {
            MoveToPlayer();

            _dashTimer += Time.fixedDeltaTime;

            if (_dashTimer >= _dashDelay)
            {
                _dashTimer = 0f;
                DoRandomDash();
            }
        }

        private void DoRandomDash()
        {
            if (_player == null) return;
            
            Vector2 moveDir = (_player.transform.position - transform.position).normalized;
            
            bool goLeft = Random.Range(0, 2) == 0;

            Vector2 perpDir;

            if (goLeft)
                perpDir = new Vector2(-moveDir.y, moveDir.x);
            else
                perpDir = new Vector2(moveDir.y, -moveDir.x);
            
            perpDir.Normalize();
            transform.position += (Vector3)(perpDir * _dashDistance);
        }

        protected override void Idle()
        {
            base.Idle();
            _dashTimer = 0f;
        }
    }
}