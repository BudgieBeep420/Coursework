using System;
using System.Collections;
using System.Collections.Generic;
using Actual_Game_Files.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCanvasScript : MonoBehaviour
{
    private string _currentSceneName;

    private AudioSource _thisAudioSource;
    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        _thisAudioSource = gameObject.GetComponent<AudioSource>();
        _currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene name is: " + _currentSceneName);
    }

    public void RestartMap()
    {
        SceneManager.LoadScene(_currentSceneName);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    
    public void PlayHoverSound()
    {
        _audioManager.Play("ButtonHoverSound", _thisAudioSource);
    }

    public void PlayClickSound()
    {
        _audioManager.Play("ButtonClickSound", _thisAudioSource);
    }
}
