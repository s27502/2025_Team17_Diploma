using System;
using System.Collections;
using System.Collections.Generic;
using Interactables;
using UnityEngine;

public class BlessingManager : MonoBehaviour
{
    [SerializeField] private BlessingChoiceUI _blessingManager;
    private void Awake()
    {
        ServiceLocator.Instance.Register(this);
        
    }

    public void ShowUI()
    {
        _blessingManager.ShowUI();
    }

    public void SetBlessings(Blessing b1,  Blessing b2)
    {
        _blessingManager.SetBlessings(b1, b2);
    }
}
