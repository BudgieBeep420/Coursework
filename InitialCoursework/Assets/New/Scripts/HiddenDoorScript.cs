using System;
using System.Collections;
using System.Collections.Generic;
using Actual_Game_Files.Scripts;
using UnityEngine;

public class HiddenDoorScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioManager audioManager;

    private const string NameOfSound = "HiddenDoorSound";

    public void StartHiddenDoorSound()
    {
        Debug.Log("Starting sound");
        audioManager.Play(NameOfSound, audioSource);
    }

    public void DisableDoor()
    {
        gameObject.SetActive(false);
    }
}
