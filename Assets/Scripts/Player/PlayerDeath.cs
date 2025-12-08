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
            
            Canvas canvas = _deathScreenInstance.GetComponentInChildren<Canvas>();
            Camera cam = Camera.main;
            if (cam != null)
            {
                canvas.worldCamera = cam;
            }

            yield return new WaitForSecondsRealtime(2f);

            Destroy(gameObject);
            ServiceLocator.Instance.Erase();
            SceneManager.LoadScene("MainMenu");

            Time.timeScale = 1;
        }
    }
}