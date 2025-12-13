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

            for (int i = 0; i < 10; i++)
            {
                bool goLeft = Random.Range(0, 2) == 0;

                Vector2 perpDir = goLeft
                    ? new Vector2(-moveDir.y, moveDir.x)
                    : new Vector2(moveDir.y, -moveDir.x);

                perpDir.Normalize();

                Vector2 startPos = transform.position;
                Vector2 targetPos = startPos + perpDir * _dashDistance;

                if (PathBlocked(startPos, targetPos))
                    continue;

                if (IsObstacleAt(targetPos))
                    continue;

                transform.position = targetPos;
                return;
            }
        }


        private bool IsObstacleAt(Vector2 pos)
        {
            Collider2D col = Physics2D.OverlapPoint(pos);
            return col != null && col.CompareTag("Obstacle");
        }

        
        private bool PathBlocked(Vector2 from, Vector2 to)
        {
            RaycastHit2D hit = Physics2D.Linecast(from, to);

            return hit.collider != null && hit.collider.CompareTag("Obstacle");
        }


        protected override void Idle()
        {
            base.Idle();
            _dashTimer = 0f;
        }
    }
}