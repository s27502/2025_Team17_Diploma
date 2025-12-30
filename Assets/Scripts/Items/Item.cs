using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private string itemName;
        [SerializeField] private int itemPrice;
        private Sprite _sprite;
        protected ItemManager ItemManager;
        [SerializeField] protected bool IsShop;
        //Stats should be in order: HP, MAX HP, DMG, ATTACK SPEED, LUCK, PROJECTILE COUNT;
        [SerializeField] private List<float> stats;
        [SerializeField] protected GameObject _price;
        [SerializeField] protected bool _isPoisoning;
        [SerializeField] protected bool _isBouncing;
        [SerializeField] protected bool _isHoming;
        private void Awake()
        {
            
            
        }

        protected virtual void Start()
        {
            ItemManager = ServiceLocator.Instance.GetService<ItemManager>();
            _sprite = GetComponent<SpriteRenderer>().sprite;
        }

        public List<float> GetStats()
        {
            return stats;
        }
        
        public bool GetPoison() => _isPoisoning;
        public bool GetBounce() => _isBouncing;
        public bool GetHoming() => _isHoming;
        
        public string GetItemName()
        {
            return itemName;
        }

        public int GetItemPrice()
        {
            return itemPrice;
        }

        public Sprite GetSprite()
        {
            return _sprite;
        }

        public void SetPrice(GameObject p)
        {
            _price = p;
        }
        public void SetIsShop(bool isShop)
        {
            IsShop = isShop;
        }
    }
}
