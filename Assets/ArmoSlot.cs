using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorSlot : MonoBehaviour
{
    [SerializeField] private Image img;

    [SerializeField] private Sprite full;
    [SerializeField] private Sprite empty;

    public void SetState(int armor) 
    {
        switch (armor)
        {
            case 0:
                img.sprite = empty;
                break;
            case 1:
                img.sprite = full;
                break;
        }
    }
}
