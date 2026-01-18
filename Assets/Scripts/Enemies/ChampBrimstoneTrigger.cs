using UnityEngine;

namespace Enemies
{
    public class ChampBrimstoneTrigger : BrimstoneTrigger
    {
        private HieraFollowerChamp _followerChamp;

        protected override void Awake()
        {
            _followerChamp = myEnemy as HieraFollowerChamp;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Collisoin");
                _followerChamp.ActivateLaser();
            }
        }
    }
}