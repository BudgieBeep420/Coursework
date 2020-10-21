using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Actual_Game_Files.Scripts.Serializable;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    private string _levelDataPath;
    private UnlockedLevelData _unlockedLevelData;
    private void Awake()
    {
        _levelDataPath = Application.dataPath + @"\Settings\UnlockedLevelData.json";
        _unlockedLevelData = JsonUtility.FromJson<UnlockedLevelData>(File.ReadAllText(_levelDataPath));
    }

    public void FinishedTutorial()
    {
        _unlockedLevelData.hasCompletedTutorial = true;
        File.WriteAllText(_levelDataPath, JsonUtility.ToJson(_unlockedLevelData));
    }
}
