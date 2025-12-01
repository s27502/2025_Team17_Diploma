using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private int hp;
        [SerializeField] private int maxHp;
        [SerializeField] private int dmg;
        [SerializeField] private float atkSpeed;
        [SerializeField] private int luck;
        [SerializeField] private int projectileCount;
        [SerializeField] private int coins;
        [SerializeField] private int armorMax;
        [SerializeField] private int armor;

        public UnityEvent<int, int> OnHpChanged = new UnityEvent<int, int>();
        
        public UnityEvent<int> onDamageChanged = new UnityEvent<int>();
        public UnityEvent<float> onAtkSpdChanged = new UnityEvent<float>();
        public UnityEvent<int> onProjCountChanged = new UnityEvent<int>();
        public UnityEvent<int> onLuckChanged = new UnityEvent<int>();
        public UnityEvent<int> onCoinsChanged = new UnityEvent<int>();
        
        private UnityEvent _dead;

        void Start()
        {
            if (_dead == null)
                _dead = new UnityEvent();

            _dead.AddListener(OnEventTriggered);
        }
        
        public void ModifyHp(int value)
        {
            if (value < 0)
            {
                var iFrames = GetComponent<PlayerIFrames>();
                if (iFrames != null && iFrames.IsInvincible())
                {
                    return;
                }
            }

            hp += value;

            if (hp > maxHp) hp = maxHp;
            if (hp < 0) hp = 0;

            Debug.Log($"HP changed: {hp}/{maxHp} (value: {value})");

            OnHpChanged.Invoke(hp, maxHp);

            if (hp <= 0)
                _dead.Invoke();
        }



        public void ModifyArmor(int value)
        {
            armor += value;
        }

        private int GetMaxArmor()
        {
            return armorMax;
        }

        public void SetArmor(int value)
        {
            armor = GetMaxArmor();
        }

        public void ModifyMaxHp(int value)
        {
            maxHp = Mathf.Clamp(maxHp + value, 1, 6);
            hp = Mathf.Clamp(hp, 0, maxHp);
            OnHpChanged.Invoke(hp, maxHp);
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

        public int GetProjectileCount()
        {
            return projectileCount;
        }

        public int GetLuck()
        {
            return luck;
        }

        public int GetHp()
        {
            return hp;
        }

        public int GetMaxHp()
        {
            return maxHp;
        }

        public int GetDmg()
        {
            return dmg;
        }

        public float GetAtkSpeed()
        {
            return atkSpeed;
        }

        public int GetCoins()
        {
            return coins;
        }

        void OnEventTriggered()
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("Player Died");
        }
    }
}
