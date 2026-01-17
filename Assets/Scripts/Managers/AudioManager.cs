using System;
using ObjectPooling.AudioSources;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class AudioManager : SingletonDoNotDestroy<AudioManager>
    {
        [SerializeField] private GameObject _playerAudioSources;
        [SerializeField] private GameObject _musicAudioSources;
        [SerializeField] private GameObject _sfxAudioSources;

        public float _masterVolume = 1f;
        public float _musicVolume = 1f;
        public float _sfxVolume = 1f;
        
        private AudioSourcePlayer _playerAudioPlayer;
        private AudioSourcePlayer _sfxPlayer;

        protected override void Awake()
        {
            base.Awake();
            _playerAudioPlayer = _playerAudioSources.GetComponent<AudioSourcePlayer>();
            _sfxPlayer = _sfxAudioSources.GetComponent<AudioSourcePlayer>();
            SetUpVolumes();
        }

        private void Update()
        {
            Debug.Log("main " + _masterVolume + " music " + _musicVolume + " sfx " + _sfxVolume);
        }

        private void SetUpVolumes()
        {
            SetMusicVolume(GetMusicVolume());
            SetSFXVolume(GetSfxVolume());
            SetMasterVolume(GetMainVolume());
        }

        public void PlayPlayerAudio(AudioClip clip)
        {
            _playerAudioPlayer.PlayAudio(clip);
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip != _musicAudioSources.GetComponent<AudioSource>().clip )
            {
                _musicAudioSources.GetComponent<AudioSource>().clip = clip;
                _musicAudioSources.GetComponent<AudioSource>().Play();
            }
        }
        
        public void PlaySfx(AudioClip clip)
        {
            _sfxPlayer.PlayAudio(clip);
        }

        public void SetMusicVolume(float vol)
        {
            _musicVolume = vol;
            _musicAudioSources.GetComponent<AudioSource>().volume = vol * _masterVolume;
        }

        public void SetMasterVolume(float vol)
        {
            _masterVolume = vol;
            SetMusicVolume(_musicVolume);
            SetSFXVolume(_sfxVolume);
        }
        
        public void SetSFXVolume(float vol)
        {
            _sfxVolume = vol;
        }
        
        public float GetMainVolume() =>
            PlayerPrefs.GetFloat("Main Volume", 1f);

        public float GetMusicVolume() =>
            PlayerPrefs.GetFloat("Music Volume", 1f);

        public float GetSfxVolume() =>
            PlayerPrefs.GetFloat("Sfx Volume", 1f);
        
        private SliderManager _sliderManager;

        public void RegisterSliderManager(SliderManager manager)
        {
            if (_sliderManager != null)
                Unsubscribe();

            _sliderManager = manager;
            Subscribe();
            PushCurrentValues();
        }

        private void Subscribe()
        {
            _sliderManager.OnMasterVolumeChanged += SetMasterVolume;
            _sliderManager.OnMusicVolumeChanged += SetMusicVolume;
            _sliderManager.OnSfxVolumeChanged += SetSFXVolume;
        }

        private void Unsubscribe()
        {
            _sliderManager.OnMasterVolumeChanged -= SetMasterVolume;
            _sliderManager.OnMusicVolumeChanged -= SetMusicVolume;
            _sliderManager.OnSfxVolumeChanged -= SetSFXVolume;
        }

        private void PushCurrentValues()
        {
            SetMasterVolume(_sliderManager.GetMainVolume());
            SetMusicVolume(_sliderManager.GetMusicVolume());
            SetSFXVolume(_sliderManager.GetSfxVolume());
        }

    }
}