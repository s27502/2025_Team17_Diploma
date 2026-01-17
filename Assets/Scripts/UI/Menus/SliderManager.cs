using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UI.Menus;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public static SliderManager Instance;
    
    [SerializeField] private Slider mainVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    
    public event Action<float> OnMasterVolumeChanged;
    public event Action<float> OnMusicVolumeChanged;
    public event Action<float> OnSfxVolumeChanged;
    
    private AudioManager _audioManager;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        mainVolumeSlider.value = GetMainVolume();
        sfxVolumeSlider.value = GetSfxVolume();
        musicVolumeSlider.value = GetMusicVolume();
        
        _audioManager = GetComponentInParent<AudioManager>();
        //Add Listeners to AudioManager
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
