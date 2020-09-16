using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [Header("Text")] 
    [SerializeField] private Text difficultyText;
    [SerializeField] private Text bloodText;
    [Space] 
    
    [Header("Scrollers")] 
    [SerializeField] private Scrollbar difficultyScroller;
    [SerializeField] private Scrollbar bloodScroller;
    [Space]
    
    [SerializeField] private GameManagerScript gameManagerScript;

    public GameSettingsProfile gameSettingsProfile;
    private string _gameSettingsDirectory;
    
    private void OnEnable()
    {
        _gameSettingsDirectory = Application.dataPath + @"\Settings\GameSettings.json";
        gameSettingsProfile = JsonUtility.FromJson<GameSettingsProfile>(File.ReadAllText(_gameSettingsDirectory));
        InitializeScrollers();
    }

    private void InitializeScrollers()
    {
        difficultyScroller.value = gameSettingsProfile.difficulty;
        bloodScroller.value = gameSettingsProfile.blood;
    }

    public void UpdateDifficultyText(float value)
    {
        if (Math.Abs(value) < 0.1) difficultyText.text = "Easy";
        else if (Math.Abs(value - 0.5) < 0.1) difficultyText.text = "Medium";
        else if (Math.Abs(value - 1) < 0.1) difficultyText.text = "Hard";
    }

    public void UpdateBloodText(float value)
    {
        if (Math.Abs(value) < 0.1) bloodText.text = "No";
        else if (Math.Abs(value - 1) < 0.1) bloodText.text = "Yes";
    }
    
    public void WriteGameSettings()
    {
        File.WriteAllText(_gameSettingsDirectory, 
            JsonUtility.ToJson(new GameSettingsProfile
        {
            blood = bloodScroller.value,
            difficulty = difficultyScroller.value
        }));
    }
}
