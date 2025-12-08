using UnityEngine;

namespace Enemies
{
    public class Sobek : Boss
    {
        [SerializeField] private GameObject _rainSpawner;
        
        private Vector2 _currentRandomTarget;
        [SerializeField] private float _posChangeInterval = 0.5f;
        private float _posChangeCounter = 0f;
        [SerializeField] private float _randomMoveDistance = 2f;

        protected override void Attack()
        {
            base.Attack();
            MoveToRandomDirection();
        }

        protected override void OnHpChangedHandler(int hp, int maxHp)
        {
            base.OnHpChangedHandler(hp, maxHp);
            if (hp <= maxHp/2)
            {
                _rainSpawner.SetActive(true);
            }

            if (hp <= 0)
            {
                Destroy(_rainSpawner);
            }
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
    }
}