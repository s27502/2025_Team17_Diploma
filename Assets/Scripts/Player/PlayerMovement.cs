using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 10f;
        
        public float dashForce = 30f;
        public float dashDuration = 0.2f;
        public float dashCooldown = 1f;
        
        private bool _isDashing = false;
        private bool _canDash = true;
        
        private Vector2 _dashDirection;
        
        private Rigidbody2D _rb;
        private PlayerIFrames _playerIFrames;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerIFrames = GetComponent<PlayerIFrames>();
            _rb.drag = 0;
        }

        public void Move(Vector2 movement)
        {
            if (!_isDashing)
                _rb.velocity = movement * speed;
            
            if(movement.sqrMagnitude > 0.01f)
                _dashDirection = movement.normalized;
        }

        public void Dash()
        {
            if (_canDash)
            {
                _playerIFrames.StartCustomIFrameRoutine(dashDuration);
                StartCoroutine(DashCoroutine());
            }
        }

        private IEnumerator DashCoroutine()
        {
            _isDashing = true;
            
            _rb.velocity = _dashDirection * dashForce;
            _canDash = false;
            yield return new WaitForSeconds(dashDuration);
            _isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            _canDash = true;
        }
    }
}