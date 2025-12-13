using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPostprocess : MonoBehaviour
{

    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        Camera cam = Camera.main;
        if (cam != null)
        {
            canvas.worldCamera = cam;
        }
    }
    
}
