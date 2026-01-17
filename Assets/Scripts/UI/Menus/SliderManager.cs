using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public static SliderManager Instance { get; private set; }

    [SerializeField] private Slider mainVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    public event Action<float> OnMasterVolumeChanged;
    public event Action<float> OnMusicVolumeChanged;
    public event Action<float> OnSfxVolumeChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        mainVolumeSlider.SetValueWithoutNotify(GetMainVolume());
        musicVolumeSlider.SetValueWithoutNotify(GetMusicVolume());
        sfxVolumeSlider.SetValueWithoutNotify(GetSfxVolume());


        AudioManager.Instance.RegisterSliderManager(this);
    }

    private void OnDestroy()
    {
        //AudioManager.Instance.RegisterSliderManager(null);
        if (Instance == this)
            Instance = null;
    }



    public void SetMainVolume(float value)
    {
        PlayerPrefs.SetFloat("Main Volume", value);
        OnMasterVolumeChanged?.Invoke(value);
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("Music Volume", value);
        OnMusicVolumeChanged?.Invoke(value);
    }

    public void SetSfxVolume(float value)
    {
        PlayerPrefs.SetFloat("Sfx Volume", value);
        OnSfxVolumeChanged?.Invoke(value);
    }
    

    public float GetMainVolume() =>
        PlayerPrefs.GetFloat("Main Volume", 1f);

    public float GetMusicVolume() =>
        PlayerPrefs.GetFloat("Music Volume", 1f);

    public float GetSfxVolume() =>
        PlayerPrefs.GetFloat("Sfx Volume", 1f);
}