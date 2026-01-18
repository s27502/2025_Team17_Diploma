using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class MainMenuMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _menuMusic;
    private void Awake()
    {
        AudioManager.Instance.PlayMusic(_menuMusic);
    }
}
