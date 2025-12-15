using UnityEngine;

namespace Interactables
{
    public class Blessing : MonoBehaviour
    {
        [SerializeField] private string _desc;
        [SerializeField] private int _coins;
        [SerializeField] private float _dmgMult;
        [SerializeField] private float _atkSpdMult;
        [SerializeField] private int _projCount;
        [SerializeField] private int _hp;

        public string GetDescription()
        {
            return _desc;
        }

        public int GetCoins()
        {
            return _coins;
        }

        public float GetDmgMult()
        {
            return _dmgMult;
        }

        public float GetAtkSpdMult()
        {
            return _atkSpdMult;
        }

        public int GetProjCount()
        {
            return _projCount;
        }

        public int GetHp()
        {
            return _hp;
        }
        
    }
}
