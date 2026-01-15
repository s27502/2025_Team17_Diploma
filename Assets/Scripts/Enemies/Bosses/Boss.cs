using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class Boss : Enemy
    {
        [SerializeField] private string _name;
        [SerializeField] private TextMeshProUGUI _nameArea;

        [SerializeField] protected GameObject _healthBarArea;
        private Slider _healthBar;

        protected override void Start()
        {
            base.Start();
            _nameArea.text = _name;
            _healthBar = _healthBarArea.GetComponentInChildren<Slider>();
            UpdateHealthBar();
            EnemyStats.OnHpChanged.AddListener(OnHpChangedHandler);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected virtual void OnHpChangedHandler(int hp, int maxHp)
        {
            _healthBar.value = (float)hp / maxHp;
        }


        private void UpdateHealthBar()
        {
            _healthBar.value = (float) EnemyStats.GetHp() / EnemyStats.GetMaxHp();
        }
        
        public override void Die()
        {
            _healthBarArea.gameObject.SetActive(false);
            base.Die();
        }
    }
}