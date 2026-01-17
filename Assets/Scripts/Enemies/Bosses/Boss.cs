using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class Boss : Enemy
    {
        [SerializeField] private string _name;
        [SerializeField] private TextMeshProUGUI _nameArea;
        [SerializeField] protected GameObject heart;
        [SerializeField] protected GameObject exit;
        private bool _dead = false;

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
            if (!_dead)
            {
                base.FixedUpdate();
            }
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
            _dead = true;
            StartCoroutine(ExitCoroutine());
            //base.Die();
        }

        protected virtual IEnumerator ExitCoroutine()
        {
            _animator.SetBool("isWalking",true);
            tag = "Untagged";
            
            while (Vector2.Distance(transform.position, exit.transform.position) > 0.1f)
            {
                MoveTo(exit.transform.position);
                yield return null;
            }

            _animator.SetBool("isWalking",false);
            yield return new WaitForSeconds(1);

            heart.SetActive(true);
            base.Die();
        }

    }
}