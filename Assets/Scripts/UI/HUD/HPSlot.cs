using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSlot : MonoBehaviour
{
    [SerializeField] private Image img;

    [SerializeField] private Sprite full;
    [SerializeField] private Sprite half;
    [SerializeField] private Sprite empty;

    public void SetState(int hp) // hp = 0, 1, 2
    {
        switch (hp)
        {
            case 2:
                img.sprite = full;
                break;
            case 1:
                img.sprite = half;
                break;
            default:
                img.sprite = empty;
                break;
        }
    }
}
