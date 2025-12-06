using System;
using StatSystem;
using UnityEngine;

namespace StatusSystem
{
    public class StatusHandler : MonoBehaviour
    {
        [SerializeField] private Stats _stats;

        [Header("Poison Settings")]
        [SerializeField] private float _poisonDelay = 5f;
        private float _poisonDelayCounter = 0f;

        [SerializeField] private float _poisonDuration = 11f;
        private float _poisonDurationCounter = 0f;

        [SerializeField] private bool _poisoned = false;
        private int _poisonDmg = 1;

        [Header("Damage Popup")]
        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private float damageTextRange = 0.5f;

        private IObjectPool _damagePopupPool;
        private IObjectFactory _damagePopupFactory;


        private void Awake()
        {
            if (damageTextPrefab != null)
            {
                _damagePopupFactory = new DamagePopupFactory(damageTextPrefab);
                _damagePopupPool = new DamagePopupPool(_damagePopupFactory, 20);
            }
        }


        private void FixedUpdate()
        {
            HandlePoison();
        }


        private void HandlePoison()
        {
            if (!_poisoned) return;

            _poisonDurationCounter += Time.fixedDeltaTime;
            _poisonDelayCounter += Time.fixedDeltaTime;

            if (_poisonDelayCounter >= _poisonDelay)
            {
                _stats.ModifyHp(-_poisonDmg);
                ShowDamageText(transform.position);

                _poisonDelayCounter = 0f;
            }

            if (_poisonDurationCounter >= _poisonDuration)
            {
                _poisoned = false;
            }
        }


        public void Poison(float duration, float dmg)
        {
            if (!_poisoned)
            {
                _poisonDelayCounter = 0f;
                _poisonDurationCounter = 0f;

                _poisonDmg = (int)dmg;
                _poisoned = true;
            }

            _poisonDurationCounter = 0f;
            _poisonDuration = duration;

            _poisonDmg = (int)dmg;
        }

        public void Cleanse()
        {
            
            _poisoned = false;
            _poisonDurationCounter = 0f;
            _poisonDuration = 0f;
        }


        private void ShowDamageText(Vector3 position)
        {
            if (_damagePopupPool == null)
                return;

            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-damageTextRange, damageTextRange),
                UnityEngine.Random.Range(-damageTextRange, damageTextRange),
                0f
            );

            IPoolableObject popupObj = _damagePopupPool.GetObject();
            if (popupObj != null)
            {
                popupObj.Spawn(position + randomOffset, Vector2.zero);

                DamagePopup popup = popupObj as DamagePopup;
                popup.Setup(_poisonDmg);
            }
        }
    }
}
