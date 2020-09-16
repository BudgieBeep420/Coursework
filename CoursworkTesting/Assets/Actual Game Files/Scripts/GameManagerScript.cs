using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameSettingsProfile gameSettingsProfile;
    private string _gameSettingsDirectory;

    private void Awake()
    {
        _gameSettingsDirectory = Application.dataPath + @"\Settings\GameSettings.json";
        gameSettingsProfile = JsonUtility.FromJson<GameSettingsProfile>(File.ReadAllText(_gameSettingsDirectory));
    }
}
