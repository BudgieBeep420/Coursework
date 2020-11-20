using System.IO;
using Actual_Game_Files.Scripts.Serializable;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameSettingsProfile gameSettingsProfile;
    private string _gameSettingsDirectory;

    public VideoSettingsProfile videoSettingsProfile;
    private string _videoSettingsDirectory;

    [Header("This is cumulative btw")]
    [SerializeField] private int[] killsToProceed;
    [Space]
    [SerializeField] private GameObject[] doorsToProceed;
    [SerializeField] private bool disableDoorCheck;


    private int _numberOfKills;
    private static readonly int GoDown = Animator.StringToHash("GoDown");

    private int _currentRoom;

    public int NumberOfKills
    {
        get => _numberOfKills;
        set { _numberOfKills = value; if(!disableDoorCheck) UpdateDoors(); }
    }

    private void Awake()
    {
        _gameSettingsDirectory = Application.dataPath + @"\Settings\GameSettings.json";
        gameSettingsProfile = JsonUtility.FromJson<GameSettingsProfile>(File.ReadAllText(_gameSettingsDirectory));
        
        // Gets the video profile as well
        
        _videoSettingsDirectory = Application.dataPath + @"\Settings\VideoSettings.json";
        videoSettingsProfile = JsonUtility.FromJson<VideoSettingsProfile>(File.ReadAllText(_videoSettingsDirectory));
    }

    private void UpdateDoors()
    {
        Debug.Log(_numberOfKills);
        if (_numberOfKills != killsToProceed[_currentRoom]) return;
        doorsToProceed[_currentRoom].GetComponent<Animator>().SetTrigger(GoDown);
        _currentRoom++;
    }
}
