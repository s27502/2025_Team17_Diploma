using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlayButtonClickSound : MonoBehaviour
{
    private AudioManager _audioManager;
    [SerializeField] private AudioClip _clickSound;
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    public void PlayButtonSound()
    {
        _audioManager.PlaySfx(_clickSound);
    }
}
