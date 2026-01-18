using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject
{
    void Spawn(Vector2 position, Vector2 direction);
    void Despawn();
    void SetPool(IObjectPool pool);
    void Reparent(GameObject obj);
    void SpecialSpawn(Vector2 spawnPos, Vector2 finalDir, float additionalProjectileMult);
}
