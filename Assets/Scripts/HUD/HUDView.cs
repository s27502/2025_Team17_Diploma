using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private Image weapon;
        [SerializeField] private TMP_Text dmg;
        [SerializeField] private TMP_Text atkSpd;
        [SerializeField] private TMP_Text projCount;
        [SerializeField] private TMP_Text luck;
        [SerializeField] private TMP_Text coins;
        [SerializeField] private Sprite emptySprite;
        
        [SerializeField] private List<HPSlot> hpSlots;
        
        public void SetWeapon(Sprite sprite)
        {
            if (weapon.color.a == 0)
            {
                var color = weapon.color;
                color.a = 1;
                weapon.color = color;
            }
            weapon.sprite = sprite != null ? sprite : emptySprite; 
        }
    
        public void SetDamage(int value)
        {
            dmg.text = $"DMG: {value}";
        }

        public void SetAttackSpeed(float value)
        {
            atkSpd.text = $"ATK SPD: {value}";
        }

        public void SetProjectileCount(int value)
        {
            projCount.text = $"PROJ: {value}";
        }

        public void SetLuck(int value)
        {
            luck.text = $"LUCK: {value}";
        }

        public void SetCoins(int value)
        {
            coins.text = $"COINS: {value}";
        }

        public void SetHP(int currentHp, int maxHp)
        {
            int totalSlots = maxHp / 2;

            for (int i = 0; i < hpSlots.Count; i++)
            {
                if (i >= totalSlots)
                {
                    hpSlots[i].gameObject.SetActive(false);
                    continue;
                }

                hpSlots[i].gameObject.SetActive(true);
                
                int hpForSlot = Mathf.Clamp(currentHp - (i * 2), 0, 2);

                hpSlots[i].SetState(hpForSlot);
            }
        }
    }
}
