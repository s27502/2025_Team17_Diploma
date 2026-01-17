using System;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;

namespace Interactables
{
    public class HorusShrine : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<Blessing> _blessings;
        [SerializeField] private Blessing riskyBlessing;
        BlessingManager _choiceUI;
        private bool _blessed = false;

        private void Start()
        {
            _choiceUI = ServiceLocator.Instance.GetService<BlessingManager>();
        }

        public void OnInteract()
        {
            
            if (!_blessed)
            {
                if (_choiceUI != null)
                {
                    _choiceUI = ServiceLocator.Instance.GetService<BlessingManager>();
                }
                Random rnd = new Random();
                var first =  _blessings[rnd.Next(_blessings.Count)];
                
                _blessings.Remove(first);
                var second =  _blessings[rnd.Next(_blessings.Count)];
                _blessings.Add(first);
                if (first == riskyBlessing)
                {
                    _blessings.Remove(first);
                } else if (second == riskyBlessing)
                {
                    _blessings.Remove(second);
                }
                _choiceUI.ShowUI();
                _choiceUI.SetBlessings(first, second);
                _blessed = true;
                Time.timeScale = 0;
            }
            
        }
        
        
    }
}
