using System;
using System.IO;
using Actual_Game_Files.Scripts.Serializable;
using UnityEngine;
using UnityEngine.UIElements;

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

    private DifficultyProfile easyDifficulty;
    private DifficultyProfile mediumDifficulty;
    private DifficultyProfile hardDifficulty;

    public DifficultyProfile currentDifficulty;

    public int NumberOfKills
    {
        get => _numberOfKills;
        set { _numberOfKills = value; if(!disableDoorCheck) UpdateDoors(); }
    }

    private void Awake()
    {
        _gameSettingsDirectory = Directory.GetCurrentDirectory() + @"\Settings\GameSettings.json";
        gameSettingsProfile = JsonUtility.FromJson<GameSettingsProfile>(File.ReadAllText(_gameSettingsDirectory));
        
        // Gets the video profile as well
        
        _videoSettingsDirectory = Directory.GetCurrentDirectory() + @"\Settings\VideoSettings.json";
        videoSettingsProfile = JsonUtility.FromJson<VideoSettingsProfile>(File.ReadAllText(_videoSettingsDirectory));
        
        InitialiseDifficultyProfiles();
        InitialisePlayerHealth();
    }

    private void UpdateDoors()
    {
        Debug.Log(_numberOfKills);
        if (_numberOfKills != killsToProceed[_currentRoom]) return;
        doorsToProceed[_currentRoom].GetComponent<Animator>().SetTrigger(GoDown);
        _currentRoom++;
    }

    private void InitialiseDifficultyProfiles()
    {
        /* This defines easy difficulty */
        easyDifficulty = new DifficultyProfile
        {
            enemyDamageMult = 1,
            enemyHealthMult = 1,
            enemyReactionMult = 1,
            playerDamageMult = 1,
            playerHealthMult = 1,
            playerSpeedMult = 1
        };
        
        /* This defines medium difficulty */
        mediumDifficulty = new DifficultyProfile
        {
            enemyDamageMult = 1.2f,
            enemyHealthMult = 1.2f,
            enemyReactionMult = 0.5f,
            playerDamageMult = 0.8f,
            playerHealthMult = 0.8f,
            playerSpeedMult = 0.8f
        };
        
        /* This defines hard difficulty */
        hardDifficulty = new DifficultyProfile
        {
            enemyDamageMult = 1.4f,
            enemyHealthMult = 1.4f,
            enemyReactionMult = 0.25f,
            playerDamageMult = 0.6f,
            playerHealthMult = 0.6f,
            playerSpeedMult = 0.6f
        };
        
        /* This sets the difficulty profile to the one selected in the options before game start */
        UpdateCurrentDifficulty(gameSettingsProfile.difficulty);
    }

    public void UpdateCurrentDifficulty(float difficulty)
    {
        /* The value of difficulties are as follows 0 = easy, 0.5 = medium, 1 = hard */

        if (difficulty == 0) currentDifficulty = easyDifficulty;
        else if (Math.Abs(difficulty - 0.5) < 0.1) currentDifficulty = mediumDifficulty;
        else if (Math.Abs(difficulty - 1) < 0.1) currentDifficulty = hardDifficulty;
    }

    private void InitialisePlayerHealth()
    {
        if (GameObject.FindWithTag("Player") == null) return;
        var pbs = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviourScript>();
        if (pbs == null) return;
        pbs.health = PlayerBehaviourScript.BaseHealth * currentDifficulty.playerHealthMult;
        pbs.maxHealth = pbs.health;
    }
}
