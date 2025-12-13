using System.Collections;
using UnityEngine;

public class PlayerIFrames : MonoBehaviour
{
    [SerializeField] private float iframeDuration = 1f;

    private bool _invincible = false;

    public bool IsInvincible() => _invincible;

    public void StartIFrames()
    {
        if (_invincible) return;

        _invincible = true;
        StartCoroutine(IFrameRoutine());
    }

    private IEnumerator IFrameRoutine()
    {
        yield return new WaitForSeconds(iframeDuration);
        _invincible = false;
    }

    public void StartCustomIFrameRoutine(float duration)
    {
        StartCoroutine(CustomIFrameRoutine(duration));
    }

    private IEnumerator CustomIFrameRoutine(float duration)
    {
        _invincible = true;
        yield return new WaitForSeconds(duration);
        _invincible = false;
    }
}