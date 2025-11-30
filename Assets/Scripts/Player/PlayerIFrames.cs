using System.Collections;
using Player;
using UnityEngine;

public class PlayerIFrames : MonoBehaviour
{
    private PlayerStats _stats;
    [SerializeField] private float iFrameDuration = 1f;

    private bool _invincible;

    void Start()
    {
        _stats = GetComponent<PlayerStats>();
        _stats.OnHpChanged.AddListener(OnHpChanged);
    }

    private void OnHpChanged(int hp, int maxHp)
    {
        if (hp < maxHp && !_invincible)
        {
            StartCoroutine(IFrameRoutine(iFrameDuration));
        }
    }

    private IEnumerator IFrameRoutine(float duration)
    {
        _invincible = true;

        yield return new WaitForSeconds(duration);

        _invincible = false;
    }

    public void StartCustomIFrameRoutine(float duration)
    {
        StartCoroutine(IFrameRoutine(duration));
    }
    
    public bool IsInvincible()
    {
        return _invincible;
    }
}
