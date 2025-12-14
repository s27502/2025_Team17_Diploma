using UnityEngine;

namespace ObjectPooling.AudioSources
{
    public class AudioSourceFactory : IObjectFactory
    {
        private GameObject _prefab;

        public AudioSourceFactory(GameObject prefab)
        {
            _prefab = prefab;
        }

        public IPoolableObject CreateNew()
        {
            GameObject obj = Object.Instantiate(_prefab);
            var audioSource = obj.GetComponent<PoolableAudioSource>();
            return audioSource;
        }
    }
}