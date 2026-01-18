using UnityEngine;

namespace Enemies
{
    public class ChampBrimstoneTrigger : BrimstoneTrigger
    {
        private HieraFollowerChamp _follower;

        protected override void Awake()
        {
            _follower = myEnemy as HieraFollowerChamp;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Collisoin");
                _follower.ActivateLaser();
            }
        }
    }
}