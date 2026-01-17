using UnityEngine;

namespace UI.Menus
{
    public class SfxScript : SliderScript
    {
        public override void OnValueChanged(float value) 
        {
            SliderManager.Instance.SetSfxVolume(value);
        }
    }
}
