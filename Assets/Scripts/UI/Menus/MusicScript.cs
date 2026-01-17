using UnityEngine;

namespace UI.Menus
{
    public class MusicScript : SliderScript
    {
        public override void OnValueChanged(float value) 
        {
            SliderManager.Instance.SetMusicVolume(value);
        }
    }
}
