using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class SliderScript : MonoBehaviour
    {
        public Slider slider;

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }
    
        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        public virtual void OnValueChanged(float value) 
        {
            PlayerPrefs.SetFloat("Slider Value", value);
        }
    }
}
