using System.Collections;
using Player;
using UnityEngine;

namespace ObjectPooling.AudioSources
{
    public class PoolableAudioSource : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private AudioSource _audioSource;
        
        private IObjectPool _pool;
        
        public void SetPool(IObjectPool pool) => _pool = pool;
        
        public void Spawn(Vector2 position, Vector2 direction)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }
        
        public void Spawn(Transform parent, Vector2 position)
        {
            transform.SetParent(parent, false);
            transform.position = position;
            gameObject.SetActive(true);
        }


        public void Despawn()
        {
            gameObject.SetActive(false);
            
        }

        public void PlayClip(AudioClip clip)
        {
            StartCoroutine(PlayClipCoroutine(clip));
        }

        private IEnumerator PlayClipCoroutine(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
            
            yield return new WaitWhile(() => _audioSource.isPlaying);
            
            _pool?.ReleaseObject(this);
        }

    }
}