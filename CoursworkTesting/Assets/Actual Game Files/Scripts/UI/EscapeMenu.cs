using System;
using System.Collections;
using System.Collections.Generic;
using Actual_Game_Files.Scripts.UI;
using Lean.Transition.Method;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EscapeMenu : BaseMenu
{
    [Header("GameObjects")]
    [SerializeField] private PlayerMovement playerMovement;
    [Space] 
    
    [Header("Panels")] 
    [SerializeField] private GameObject[] panels;
    
    protected override GameObject[] PanelArray { get; set; }

    private void OnEnable()
    {
        PanelArray = panels;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ReturnToGame()
    {
        playerMovement.ReturnToGame();
    }
    
}
