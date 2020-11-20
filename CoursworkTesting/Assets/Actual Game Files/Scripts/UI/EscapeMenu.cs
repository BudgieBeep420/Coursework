using System;
using Actual_Game_Files.Scripts;
using Actual_Game_Files.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : BaseMenu
{
    [Header("GameObjects")]
    [SerializeField] private PlayerMovement playerMovement;
    [Space] 
    
    [Header("Panels")] 
    [SerializeField] private GameObject[] panels;
    [Space]
    
    [SerializeField] private AudioManager audioManager;
    
    protected override GameObject[] PanelArray { get; set; }
    protected override AudioManager AudioManager { get; set; }
    protected override AudioSource ThisAudioSource { get; set; }

    private void OnEnable()
    {
        PanelArray = panels;
        AudioManager = audioManager;
        ThisAudioSource = gameObject.GetComponent<AudioSource>();
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
