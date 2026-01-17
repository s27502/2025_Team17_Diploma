using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nahebeku : Enemy
{
    [SerializeField] private GameObject snakeSpawner;
    private float _shootCounter;
    private bool _shootX = true;
    protected override void Attack()
    {
        NavMoveTo(_player.transform);
        Shoot();
    }

    private void Shoot()
    {
        if (_shootCounter <= EnemyStats.GetFireRate())
        {
            _shootCounter += Time.fixedDeltaTime;
        }
        else
        {
            switch (_shootX)
            {
                case true:
                    ShootX();
                    break;
                case false:
                    ShootCross();
                    break;
            }

            _shootX = !_shootX;
            _shootCounter = 0f;
        }
    }
    
    private void ShootCross()
    {
        Vector2[] dirs =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        foreach (var dir in dirs)
            projectileFactory.Shoot(dir);
    }

    private void ShootX()
    {
        Vector2[] dirs =
        {
            new Vector2(1, 1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(-1, -1).normalized
        };

        foreach (var dir in dirs)
            projectileFactory.Shoot(dir);
    }

    public override void Die()
    {
        Destroy(snakeSpawner);
        base.Die();
    }
}
