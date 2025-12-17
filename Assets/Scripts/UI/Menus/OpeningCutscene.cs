using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OpeningCutscene : Dialogue
{
    [SerializeField] private AudioClip _openingMusic;

    [SerializeField] private Animator _transition;
    [SerializeField] private GameObject _imageObject;
    [SerializeField] private List<CutsceneImage> _images;

    protected override void Start()
    {
        base.Start();
        AudioManager.Instance.PlayMusic(_openingMusic);
    }

protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(LoadMenu());
        }

        CutsceneImage data = _images.Find(e => e.index == index);
        if (data != null)
        {
            _imageObject.GetComponent<Image>().sprite = data.sprite;
        }
    }

    protected override void EndDialogue()
    {

        StartCoroutine(LoadMenu());
    }

    private IEnumerator LoadMenu()
    {
        _transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        base.EndDialogue();
        SceneManager.LoadScene("MainMenu");
    }
}
