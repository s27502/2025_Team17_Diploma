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

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>().sprite;
            
        }

        private void Start()
        {
            ItemManager = ServiceLocator.Instance.GetService<ItemManager>();
        }

        public List<float> GetStats()
        {
            return stats;
        }
        
        
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
    }
}
