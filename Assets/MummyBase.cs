using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyBase : Enemy
{
    [SerializeField] private GameObject mummyPile;

    public override void Die()
    {
        mummyPile.SetActive(true);
        gameObject.SetActive(false);
        //base.Die();
    }

    public void Respawn()
    {
        EnemyStats.ModifyHp(EnemyStats.GetMaxHp()/2);
    }
}
