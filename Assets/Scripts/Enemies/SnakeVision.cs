using UnityEngine;

namespace Enemies
{
    public class SnakeVision : EnemySight
    {
        private Snake _snake;
        protected override void Start()
        {
            base.Start();
            _snake = Enemy as Snake;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _snake.StartDashing(other.gameObject.transform.position);
            }
        }
        
        protected override void OnTriggerExit2D(Collider2D other)
        {
            
        }
    }
}