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
            _deathScreenInstance = Instantiate(_deathScreen);
            yield return new WaitForSecondsRealtime(2f);
            Destroy(gameObject);
            ServiceLocator.Instance.Erase();
            SceneManager.LoadScene("MainMenu");
            //_deathScreen.SetActive(false);
            Time.timeScale = 1;

        }
    }
}