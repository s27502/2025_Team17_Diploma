using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Floor 1");
    }

    public void ExitButton()
    {
        Application.Quit();
        if (Application.isEditor)
        {
            Debug.Log("Exit");
        }
    }
}
