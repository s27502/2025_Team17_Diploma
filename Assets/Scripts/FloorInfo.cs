using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    
    public RectTransform rect;

    public Vector2 offscreenStart = new Vector2(-1500, 0);
    public Vector2 onscreenPos = Vector2.zero;
    public Vector2 offscreenEnd = new Vector2(1500, 0);

    public float flyInTime = 0.5f;
    public float flyOutTime = 0.5f;

    Coroutine currentRoutine;
    
    void Awake()
    {
        rect.anchoredPosition = offscreenStart;
    }
    
    public void FlyIn()
    {
        StartMove(onscreenPos, flyInTime);
    }

    public void FlyInNOut()
    {
        FlyIn();
        StartCoroutine(Wait());
        
    }
    
    public void FlyOut()
    {
        StartMove(offscreenEnd, flyOutTime);
    }

    void StartMove(Vector2 target, float duration)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(MoveRoutine(target, duration));
    }
    
    public void SetText(string text)
    {
        textField.text = text;
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        FlyOut();
    }
    
    IEnumerator MoveRoutine(Vector2 target, float time)
    {
        Vector2 start = rect.anchoredPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            rect.anchoredPosition = Vector2.Lerp(start, target, EaseOut(t));
            yield return null;
        }

        rect.anchoredPosition = target;
    }

    float EaseOut(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }
}
