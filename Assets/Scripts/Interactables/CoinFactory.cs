using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFactory : MonoBehaviour
{
    public GameObject smallCoinPrefab;
        public GameObject bigCoinPrefab;

        public float launchForce = 5f;
        public float upwardBias = 0.5f;
        
        public void SpawnCoins(Vector3 pos, int totalValue)
        {
            int smallCoins = 0;
            int bigCoins = 0;
            int rest;
            if (totalValue >= 5)
            {
                rest = totalValue - 5 * (totalValue / 5);
                for (int i = 0; i < totalValue/5; i++)
                {
                    InstantiateAndLaunch(bigCoinPrefab, pos);
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
                    InstantiateAndLaunch(smallCoinPrefab, pos);
                    smallCoins += 1;
                }
            }
        }

        private void InstantiateAndLaunch(GameObject prefab, Vector3 pos)
        {
            GameObject coin = Instantiate(prefab, pos, Quaternion.identity);

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f; 
           // float angle = Random.Range(0f, 2f * Mathf.PI);
            //Vector2 randomDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                
            //rb.AddForce(randomDir * launchForce, ForceMode2D.Impulse);
        }
}
