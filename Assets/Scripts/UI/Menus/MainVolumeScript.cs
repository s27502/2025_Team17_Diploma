using UnityEngine;

namespace UI.Menus
{
    public class MainVolumeScript : SliderScript
    {
        public override void OnValueChanged(float value) 
        {
            SliderManager.Instance.SetMainVolume(value);
        }
    }
}
