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
            StartCoroutine(IFrameRoutine());
        }
    }

    private IEnumerator IFrameRoutine()
    {
        _invincible = true;

        yield return new WaitForSeconds(iFrameDuration);

        _invincible = false;
    }

    public bool IsInvincible()
    {
        return _invincible;
    }
}
