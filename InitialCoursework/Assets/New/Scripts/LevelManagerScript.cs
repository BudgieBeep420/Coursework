using System.IO;
using Actual_Game_Files.Scripts.Serializable;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    private string _levelDataPath;
    private UnlockedLevelData _unlockedLevelData;
    
    /* This part is called at the start of the scene, it gets the information from the saved file */
    private void Awake()
    {
        _levelDataPath = Directory.GetCurrentDirectory() + @"\Settings\UnlockedLevelData.json";
        _unlockedLevelData = JsonUtility.FromJson<UnlockedLevelData>(File.ReadAllText(_levelDataPath));
    }

    /* These functions are called once each level is finished. It writes the fact they have finished to the file*/
    public void FinishedTutorial()
    {
        _unlockedLevelData.hasCompletedTutorial = true;
        File.WriteAllText(_levelDataPath, JsonUtility.ToJson(_unlockedLevelData));
    }
    
    public void FinishedM1()
    {
        _unlockedLevelData.hasCompletedM1 = true;
        File.WriteAllText(_levelDataPath, JsonUtility.ToJson(_unlockedLevelData));
    }
}
