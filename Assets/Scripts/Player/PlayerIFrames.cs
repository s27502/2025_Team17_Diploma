using System.Collections;
using Player;
using UnityEngine;

public class PlayerIFrames : MonoBehaviour
{
    private PlayerStats stats;
    [SerializeField] private float iFrameDuration = 1f;

    private bool _invincible;

    void Start()
    {
        stats = gameObject.GetComponent<PlayerStats>();
        stats.OnHpChanged.AddListener(OnHpChanged);
    }

    private void OnHpChanged(int hp, int maxHp)
    {
        if (hp <= 0) return;
        
        if (!_invincible)
            StartCoroutine(IFrameRoutine());
    }

    private IEnumerator IFrameRoutine()
    {
        _invincible = true;
        
        Debug.Log("I-FRAMES ON");

        yield return new WaitForSeconds(iFrameDuration);

        _invincible = false;
        Debug.Log("I-FRAMES OFF");
    }

    public bool IsInvincible()
    {
        return _invincible;
    }
}