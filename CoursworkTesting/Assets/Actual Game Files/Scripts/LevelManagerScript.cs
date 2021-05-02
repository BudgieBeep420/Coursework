using System.IO;
using Actual_Game_Files.Scripts.Serializable;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    private string _levelDataPath;
    private UnlockedLevelData _unlockedLevelData;
    
    private void Awake()
    {
        _levelDataPath = Directory.GetCurrentDirectory() + @"\Settings\UnlockedLevelData.json";
        _unlockedLevelData = JsonUtility.FromJson<UnlockedLevelData>(File.ReadAllText(_levelDataPath));
    }

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
    
    public void FinishedM2()
    {
        _unlockedLevelData.hasCompletedM2 = true;
        File.WriteAllText(_levelDataPath, JsonUtility.ToJson(_unlockedLevelData));
    }
    
    public void FinishedM3()
    {
        _unlockedLevelData.hasCompletedM3 = true;
        File.WriteAllText(_levelDataPath, JsonUtility.ToJson(_unlockedLevelData));
    }
}
