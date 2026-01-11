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
        }
        
        public void SetSFXVolume(float vol)
        {
            _sfxVolume = vol;
        }
    }
}