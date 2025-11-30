using UnityEngine;
using Enemies;
using Managers;
using Player;

public class PlayerProjectile : ProjectileBase
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private float damageTextRange = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        damage = ServiceLocator.Instance.GetService<PlayerStatManager>()
            .GetPlayerStats().GetDmg();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        if (other.CompareTag("Enemy"))
        {
            var enemyStats = other.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.ModifyHp(-damage);
                ShowDamageText(other.transform.position);
            }
            _pool?.ReleaseObject(this);
            return;
        }

        _pool?.ReleaseObject(this);
    }

    private void ShowDamageText(Vector3 enemyPos)
    {
        if (damageTextPrefab == null)
            return;

        Vector3 randomOffset = new Vector3(
            Random.Range(-damageTextRange, damageTextRange),
            Random.Range(-damageTextRange, damageTextRange),
            0f
        );

        GameObject textObj = Instantiate(damageTextPrefab, enemyPos + randomOffset, Quaternion.identity);
        var dmgPopup = textObj.GetComponent<DamagePopup>();
        if (dmgPopup != null)
        {
            dmgPopup.Setup(damage);
        }
    }
}