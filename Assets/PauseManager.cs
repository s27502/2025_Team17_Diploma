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
    private void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        hud.SetActive(false);
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
        pauseMenu.SetActive(false);
        _player.GetComponent<PlayerDeath>().StartDeath();
        //SceneManager.LoadScene("MainMenu");
    }
}
