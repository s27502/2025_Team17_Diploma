using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 10f;

        private Rigidbody2D _rb;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = 0;
        }

        public void Move(Vector2 movement)
        {
            _rb.velocity = movement * speed;
        }
    }
}