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

        [SerializeField] private Slider _healthBar;

        protected override void Start()
        {
            base.Start();
            _nameArea.text = _name;
            UpdateHealthBar();
            EnemyStats.OnHpChanged.AddListener(OnHpChangedHandler);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Debug.Log(EnemyStats.GetHp());
        }

        protected virtual void OnHpChangedHandler(int hp, int maxHp)
        {
            _healthBar.value = (float)hp / maxHp;
        }


        private void UpdateHealthBar()
        {
            _healthBar.value = (float) EnemyStats.GetHp() / EnemyStats.GetMaxHp();
        }
    }
}