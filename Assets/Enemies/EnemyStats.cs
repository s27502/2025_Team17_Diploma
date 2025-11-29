using UnityEngine;

namespace Enemies
{
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField] private int _maxHp = 6;
        [SerializeField] private int _hp = 6;
        private int _dmg = 1;
        [SerializeField] private float _atkSpd = 1f;
        [SerializeField] private float _fireRate = 1f;
        [SerializeField] private float _movementSpeed = 1f;

        public int MaxHp
        {
            get => _maxHp;
            set => _maxHp = value;
        }

        public int Hp
        {
            get => _hp;
            set => _hp = value;
        }

        public int Dmg
        {
            get => _dmg;
            set => _dmg = value;
        }

        public float AtkSpd
        {
            get => _atkSpd;
            set => _atkSpd = value;
        }

        public float FireRate
        {
            get => _fireRate;
            set => _fireRate = value;
        }

        public float MovementSpeed
        {
            get => _movementSpeed;
            set => _movementSpeed = value;
        }
        
        public void SetHp(int newHp)
        {
            Hp = Mathf.Clamp(newHp, 0, MaxHp);
        }

        public void ModifyHp(int amount)
        {
            Hp = Mathf.Clamp(Hp + amount, 0, MaxHp);
        }

        public void ModifyMaxHp(int amount)
        {
            MaxHp += amount;
        }

        public void ModifyFireRate(float amount)
        {
            FireRate += amount;
        }

        public void ModifyDamage(int amount)
        {
            Dmg += amount;
        }

        public void ModifyAtkSpd(float amount)
        {
            AtkSpd += amount;
        }
    }
}