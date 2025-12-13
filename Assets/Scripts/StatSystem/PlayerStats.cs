using System;
using StatSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerStats : Stats
    {
        [Header("Player Stats")]
        [SerializeField] private int dmg;
        [SerializeField] private float atkSpeed;
        [SerializeField] private int luck;
        [SerializeField] private int projectileCount;
        [SerializeField] private int coins;
        [SerializeField] private int armorMax;
        [SerializeField] private int armor;
        private PlayerDeath _death;
        
        public UnityEvent<int, int> OnHpChanged = new UnityEvent<int, int>();
        public UnityEvent<int> onDamageChanged = new UnityEvent<int>();
        public UnityEvent<float> onAtkSpdChanged = new UnityEvent<float>();
        public UnityEvent<int> onProjCountChanged = new UnityEvent<int>();
        public UnityEvent<int> onLuckChanged = new UnityEvent<int>();
        public UnityEvent<int> onCoinsChanged = new UnityEvent<int>();
        public UnityEvent<int, int> onArmorChanged = new UnityEvent<int, int>();

        private PlayerIFrames _iFrames;

        private void Start()
        {
            _death = GetComponent<PlayerDeath>();
            _iFrames = GetComponent<PlayerIFrames>();
        }

        public override void ModifyHp(int value)
        {
            if (value < 0)
            {
                if (_iFrames != null && _iFrames.IsInvincible())
                    return;

                _iFrames?.StartIFrames();
            }
            
            if (armor > 0)
            {
                ModifyArmor(value);
                OnHpChanged?.Invoke(_hp, _maxHp);
                return;
            }
            
            base.ModifyHp(value);
            OnHpChanged?.Invoke(_hp, _maxHp);
        }

        public override void StatusDmg(int amount)
        {
            base.StatusDmg(amount);
            OnHpChanged?.Invoke(_hp, _maxHp);
        }

        public override void ModifyMaxHp(int value)
        {
            base.ModifyMaxHp(value);
            _hp += value;
            OnHpChanged?.Invoke(_hp, _maxHp);
        }

        protected override void OnDeath()
        {
            _death.StartDeath();
        }

        public void ModifyDmg(int value)
        {
            dmg += value;
            onDamageChanged?.Invoke(dmg);
        }

        public void ModifyAttackSpeed(float value)
        {
            atkSpeed = Mathf.Clamp(atkSpeed + value, 0, 10);
            onAtkSpdChanged?.Invoke(atkSpeed);
        }

        public void ModifyCoins(int value)
        {
            coins += value;
            onCoinsChanged?.Invoke(coins);
        }

        public void ModifyProjectileCount(int value)
        {
            projectileCount = Mathf.Clamp(projectileCount + value, 1, 5);
            onProjCountChanged?.Invoke(projectileCount);
        }

        public void ModifyLuck(int value)
        {
            luck += value;
            onLuckChanged?.Invoke(luck);
        }

        public void ModifyArmorMax(int value)
        {
            armorMax = Mathf.Clamp(armorMax + value, 0, 5);
        }

        public void ModifyArmor(int value)
        {
            armor = Mathf.Clamp(armor + value, 0, armorMax);
            onArmorChanged?.Invoke(armor, armorMax);
        }

        public int GetArmorMax() => armorMax;
        public int GetArmor() => armor;

        public int GetProjectileCount() => projectileCount;
        public int GetLuck() => luck;
        public int GetDmg() => dmg;
        public float GetAtkSpeed() => atkSpeed;
        public int GetCoins() => coins;
    }
}
