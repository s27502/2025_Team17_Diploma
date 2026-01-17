using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MummyPile : Enemy
{
    [SerializeField] private GameObject motherMummy;
    [SerializeField] private GameObject mummyBase;
    [SerializeField] private float respawnTime;
    private float counter = 0f;
    private MummyBase _mummyBase;

    private void Start()
    {
        _mummyBase = mummyBase.GetComponent<MummyBase>();
        counter = 0f;
    }

    private void Awake()
    {
        _mummyBase = mummyBase.GetComponent<MummyBase>();
        counter = 0f;
    }
    

    protected override void FixedUpdate()
    {
        Debug.Log("imAlive");
        if (counter <= respawnTime)
        {
            counter += Time.fixedDeltaTime;
        }
        else
        {
            _mummyBase.Respawn();
            mummyBase.SetActive(true);
            counter = 0f;
            gameObject.SetActive(false);
        }
    }

    public override void Die()
    {
        Destroy(motherMummy);
    }
}
