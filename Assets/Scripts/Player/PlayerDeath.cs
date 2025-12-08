using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private GameObject _deathScreen;
        private GameObject _deathScreenInstance;

        public void StartDeath()
        {
            StartCoroutine(DeathCoroutine());
        }

        private IEnumerator DeathCoroutine()
        {
            Time.timeScale = 0;
            Destroy(ServiceLocator.Instance.GetService<FloorManager>().GetCurrentRoom());
            
            _deathScreenInstance = Instantiate(_deathScreen);
            
            yield return new WaitForSecondsRealtime(2f);

            Destroy(gameObject);
            ServiceLocator.Instance.Erase();
            SceneManager.LoadScene("MainMenu");

            Time.timeScale = 1;
        }
    }
}