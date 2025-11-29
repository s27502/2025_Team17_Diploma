using UnityEngine;

namespace Interactables
{
    public class CoinFactory : MonoBehaviour
    {
        public GameObject smallCoinPrefab;
        public GameObject bigCoinPrefab;

    
    
        public void SpawnCoins(Vector3 pos, int totalValue, GameObject parent)
        {
            int smallCoins = 0;
            int bigCoins = 0;
            int rest;
            if (totalValue >= 5)
            {
                rest = totalValue - 5 * (totalValue / 5);
                for (int i = 0; i < totalValue/5; i++)
                {
                    InstantiateAndLaunch(bigCoinPrefab, pos, parent);
                    bigCoins += 1;
                }

            }
            else
            {
                rest = totalValue;
            }

            if (rest > 0)
            {
                for (int i = 0; i < rest; i++)
                {
                    InstantiateAndLaunch(smallCoinPrefab, pos, parent);
                    smallCoins += 1;
                }
            }
        }

        private void InstantiateAndLaunch(GameObject prefab, Vector3 pos, GameObject parent)
        {
            GameObject coin = Instantiate(prefab, pos, Quaternion.identity);
            coin.transform.SetParent(parent.transform);
        
            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f; 
            rb.drag = 3f;

            float angle = Random.Range(0f, 2f * Mathf.PI);
            Vector2 randomDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        
            float force = Random.Range(2f, 6f);
            rb.AddForce(randomDir * force, ForceMode2D.Impulse);
        }
    }
}
