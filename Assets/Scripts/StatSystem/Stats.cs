using UnityEngine;

namespace StatSystem
{
    public abstract class Stats : MonoBehaviour
    {
        [SerializeField] protected int _hp;
        [SerializeField] protected int _maxHp;

        public virtual void ModifyHp(int amount)
        {
            _hp = Mathf.Clamp(_hp + amount, 0, _maxHp);

            if (_hp <= 0)
                OnDeath();
        }

        public virtual void StatusDmg(int amount)
        {
            _hp = Mathf.Clamp(_hp + amount, 1, _maxHp);
        }

        public virtual void ModifyMaxHp(int amount)
        {
            _maxHp += amount;
            if (_maxHp < 1) _maxHp = 1;

            if (_hp > _maxHp)
                _hp = _maxHp;
        }

        protected virtual void OnDeath()
        {
            Debug.Log($"{gameObject.name} died");
        }

        public int GetHp() => _hp;
        public int GetMaxHp() => _maxHp;
    }
}