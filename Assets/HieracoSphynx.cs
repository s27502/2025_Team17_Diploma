using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HieracoSphynx : Enemy
{
    [SerializeField] private GameObject laserObject;
    [SerializeField] private float laserDelay = 1f;
    [SerializeField] private float hiddenLaserDelay = .5f;
    [SerializeField] private float laserDuration = 1f;
    private float laserCounter = 0f;
    private float laserDurationCounter = 0f;
    private float hiddenLaserDelayCounter = 0f;

    private float fireCounter = 0f;

    protected override void Attack()
    {
        if (fireCounter <= EnemyStats.GetFireRate())
        {
            Debug.Log("searching");
            fireCounter += Time.fixedDeltaTime;
        }
        else
        {
            ShootLaser();
        }
    }

    private void ShootLaser()
    {
        if (laserCounter == 0)
        {
            //start anim

        }
        if (laserCounter <= laserDelay)
        {
            laserCounter += Time.fixedDeltaTime;
        }
        else
        {
            if (hiddenLaserDelayCounter == 0)
            {
                RotateLaserTowardsPlayer();
            }
            if (hiddenLaserDelayCounter <= hiddenLaserDelay)
            {
                hiddenLaserDelayCounter += Time.fixedDeltaTime;
            }
            else
            {
                if (laserDurationCounter == 0)
                {
                    laserObject.SetActive(true);
                }

                if (laserDurationCounter <= laserDuration)
                {
                    laserDurationCounter += Time.fixedDeltaTime;
                }
                else
                {
                    laserObject.SetActive(false);
                    fireCounter = 0;
                    laserCounter = 0;
                    laserDurationCounter = 0;
                    hiddenLaserDelayCounter = 0;
                }
            }

        }
    }

    private void RotateLaserTowardsPlayer()
    {
        Vector2 dir = (_player.transform.position - laserObject.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        laserObject.transform.rotation = Quaternion.Euler(0f,0f, angle);
    }
    
}
