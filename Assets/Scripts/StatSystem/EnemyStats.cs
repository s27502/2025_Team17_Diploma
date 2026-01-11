using StatSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class EnemyStats : Stats
    {
        [Header("Enemy Stats")]
        [SerializeField] private int dmg = 1;
        [SerializeField] private float atkSpd = 1f;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float movementSpeed = 1f;
        
        public UnityEvent<int, int> OnHpChanged = new UnityEvent<int, int>();

        protected override void OnDeath()
        {
            Debug.Log($"{name} enemy died!");
            GetComponent<Enemy>().Die();
            //Destroy(gameObject);
        }

        public void ModifyDamage(int amount) => dmg += amount;
        public void ModifyFireRate(float amount) => fireRate += amount;
        public void ModifyAtkSpd(float amount) => atkSpd += amount;
        public void ModifyMovement(float amount) => movementSpeed += amount;

        public override void ModifyHp(int value)
        {
            base.ModifyHp(value);
            OnHpChanged?.Invoke(_hp,_maxHp);
        }

        public int GetDmg() => dmg;
        public float GetAtkSpd() => atkSpd;
        public float GetFireRate() => fireRate;
        public float GetMovementSpeed() => movementSpeed;

        public void SetMovementSpeed(float speed)
        {
            movementSpeed = speed;
        }
    }
}