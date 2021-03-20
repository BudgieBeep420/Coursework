using System;
using System.IO;
using Actual_Game_Files.Scripts.Serializable;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject firstButton;
    [SerializeField] private GameObject secondButton;
    [SerializeField] private GameObject thirdButton;
    

    private void OnEnable()
    {
        var levelUnlockedDirectory = Directory.GetCurrentDirectory() + @"\Settings\UnlockedLevelData.json";
        var levelUnlockedData = JsonUtility.FromJson<UnlockedLevelData>(File.ReadAllText(levelUnlockedDirectory));

        if (levelUnlockedData.hasCompletedTutorial) firstButton.SetActive(true);
        if (levelUnlockedData.hasCompletedM1) secondButton.SetActive(true);
        if (levelUnlockedData.hasCompletedM2) thirdButton.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void Mission1()
    {
        SceneManager.LoadScene("LevelOne");
    }
    
    public void Mission2()
    {
        SceneManager.LoadScene("LevelTwo");
    }
    
    public void Mission3()
    {
        SceneManager.LoadScene("LevelThree");
    }
}
