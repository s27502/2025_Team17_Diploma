using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject _player;
    private bool _paused = false;
    private void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0;
        _paused = true;
    }
    
    private IEnumerator RestartCoroutine()
    {
        Time.timeScale = 0;
        Destroy(ServiceLocator.Instance.GetService<FloorManager>().GetCurrentRoom());
            
        yield return new WaitForSecondsRealtime(0f);

        Destroy(gameObject);
        ServiceLocator.Instance.Erase();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1;
    }
    
    public void Restart()
    {
        StartCoroutine(RestartCoroutine());
    }
    
    public void Resume()
    {
        hud.SetActive(true);
        pauseMenu.SetActive(false);
        _paused = false;
        Time.timeScale = 1;
    }

    public bool GetPaused()
    {
        return _paused;
    }
    
    public void Quit()
    {
        pauseMenu.SetActive(false);
        _player.GetComponent<PlayerDeath>().StartDeath();
        //SceneManager.LoadScene("MainMenu");
    }
}
