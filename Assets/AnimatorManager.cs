using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private SpriteFlipper  _spriteFlipper;
    void Awake()
    {
        ServiceLocator.Instance.Register(this);
        Debug.Log(ServiceLocator.Instance.GetService<AnimatorManager>());
    }

    private void Start()
    {
        _spriteFlipper = GetComponent<SpriteFlipper>();
    }

    public SpriteFlipper GetSpriteFlipper()
    {
        return _spriteFlipper;
    }
}
