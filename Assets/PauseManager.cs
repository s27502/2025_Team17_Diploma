using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject hud;

    void Start()
    {
        ServiceLocator.Instance.Register(this);
    }
    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }

    public void Pause()
    {
        hud.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        hud.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
