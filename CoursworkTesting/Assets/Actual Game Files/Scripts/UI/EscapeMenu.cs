using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Transition.Method;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EscapeMenu : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private PlayerMovement playerMovement;
    [Space] 
    
    [Header("Panels")] 
    [SerializeField] private GameObject[] panels;

    private enum Panels
    {
        Main,
        Options,
        Video,
        Audio,
        Game
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ReturnToGame()
    {
        playerMovement.ReturnToGame();
    }

    public void ToMainPanel()
    {
        foreach (var panel in panels) panel.SetActive(false);
        panels[(int) Panels.Main].SetActive(true);
    }

    public void ToOptionPanel()
    {
        foreach (var panel in panels) panel.SetActive(false);
        panels[(int) Panels.Options].SetActive(true);
    }

    public void ToVideoPanel()
    {
        foreach (var panel in panels) panel.SetActive(false);
        panels[(int) Panels.Video].SetActive(true);
    }
    
    public void ToAudioPanel()
    {
        foreach (var panel in panels) panel.SetActive(false);
        panels[(int) Panels.Audio].SetActive(true);
    }
    
    public void ToGamePanel()
    {
        foreach (var panel in panels) panel.SetActive(false);
        panels[(int) Panels.Game].SetActive(true);
    }
    
}
