using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Animator transition;
    public void PlayButton()
    {
        StartCoroutine(LoadFloor1Coroutine());

    }

    private IEnumerator LoadFloor1Coroutine()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
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
