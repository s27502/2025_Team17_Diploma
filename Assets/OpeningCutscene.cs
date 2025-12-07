using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OpeningCutscene : Dialogue
{
    [SerializeField] private GameObject _imageObject;

    [SerializeField] private List<CutsceneImage> _images;

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //playtransition
            SceneManager.LoadScene("MainMenu");
        }

        CutsceneImage data = _images.Find(e => e.index == index);
        if (data != null)
        {
            _imageObject.GetComponent<Image>().sprite = data.sprite;
        }
    }

    protected override void EndDialogue()
    {
        base.EndDialogue();
        //playtransition
        SceneManager.LoadScene("MainMenu");
    }
}
