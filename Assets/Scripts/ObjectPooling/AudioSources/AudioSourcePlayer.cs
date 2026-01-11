using UnityEngine;

namespace ObjectPooling.AudioSources
{
    public class AudioSourcePlayer : MonoBehaviour
    {
        [SerializeField] private GameObject _audioSourcePrefab;
        [SerializeField] private int initialPoolSize = 10;
        private float _volume = 1;
        protected IObjectPool _audioSourcePool;
        
        private void Awake()
        {
            IObjectFactory factory = new AudioSourceFactory(_audioSourcePrefab);
            _audioSourcePool = new AudioSourcePool(factory, initialPoolSize);
        }

        public void PlayAudio(AudioClip clip)
        {
            IPoolableObject source = _audioSourcePool.GetObject();
            PoolableAudioSource audioSource = (PoolableAudioSource)source;

            if (audioSource != null)
            {
                audioSource.Spawn(gameObject.transform,Vector2.zero);
                audioSource.SetVolume(_volume);
                audioSource.PlayClip(clip);
            }
        }

        public void SetVolume(float volume)
        {
            _volume *= volume;
            Debug.Log("volume " + _volume);
        }
    }
}