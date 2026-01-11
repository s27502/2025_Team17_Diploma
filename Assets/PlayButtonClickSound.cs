using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlayButtonClickSound : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private AudioClip _clickSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayShit()
    {
        _audioManager.PlaySfx(_clickSound);
    }
}
