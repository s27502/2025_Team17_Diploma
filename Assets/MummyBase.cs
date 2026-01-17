using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyBase : Enemy
{
    [SerializeField] private GameObject mummyPile;
    private float _shootCounter;

    public override void Die()
    {
        mummyPile.transform.position = transform.position;
        mummyPile.SetActive(true);
        gameObject.SetActive(false);
        //base.Die();
    }

    public void Respawn()
    {
        EnemyStats.ModifyHp(EnemyStats.GetMaxHp()/2);
    }

    protected override void Attack()
    {
        CrossMove();
        Shoot8();
    }
    
    private void Shoot8()
    {
        if (_shootCounter <= EnemyStats.GetFireRate())
        {
            _shootCounter += Time.fixedDeltaTime;
        }
        else
        {
            ShootCross();
            ShootX();
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
}
