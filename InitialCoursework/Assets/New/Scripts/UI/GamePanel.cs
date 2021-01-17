using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [Header("Text")] 
    [SerializeField] private Text difficultyText;
    [SerializeField] private Text bloodText;
    [SerializeField] private Text sensitivityText;
    [Space] 
    
    [Header("Scrollers")] 
    [SerializeField] private Scrollbar difficultyScroller;
    [SerializeField] private Scrollbar bloodScroller;
    [SerializeField] private Slider sensitivitySlider;
    [Space] 
    
    [Header("GameObjects")] 
    [SerializeField] private GameManagerScript gameManager;
    [SerializeField] private PlayerMovement playerMovement;
    [Space]

    public GameSettingsProfile gameSettingsProfile;
    private string _gameSettingsDirectory;
    
    
    /* Gets the data from the Settings folder, and sets up the toggles and sliders
        with their correct values*/
    private void OnEnable()
    {
        _gameSettingsDirectory = Directory.GetCurrentDirectory() + @"\Settings\GameSettings.json";
        gameSettingsProfile = JsonUtility.FromJson<GameSettingsProfile>(File.ReadAllText(_gameSettingsDirectory));
        InitializeScrollers();
    }

    private void InitializeScrollers()
    {
        difficultyScroller.value = gameSettingsProfile.difficulty;
        bloodScroller.value = gameSettingsProfile.blood;
        sensitivitySlider.value = gameSettingsProfile.sensitivity;
    }
    
    /* This changes all of the text values that appear next to the sliders / toggles to make
        sure they reflect what is seen*/
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

    public void UpdateSensitivityText(float value)
    {
        var text = value.ToString(CultureInfo.InvariantCulture);
        if (text.Length > 4) sensitivityText.text = text.Substring(0, 4);

        if (playerMovement == null) return;
        playerMovement.userDefinedSens = value;
        Debug.Log("playerMovementScript is null");
    }
    
    /* This writes the a new game settings profile back to memory when the back button is pressed */
    public void WriteGameSettings()
    {
        var newProfile = new GameSettingsProfile
        {
            blood = bloodScroller.value,
            difficulty = difficultyScroller.value,
            sensitivity = sensitivitySlider.value
        };
        
        File.WriteAllText(_gameSettingsDirectory, JsonUtility.ToJson(newProfile));
        UpdateGameManager(newProfile);
    }

    /* When an option is changed, the game manager needs to know, as other scripts need to access this data */
    private void UpdateGameManager(GameSettingsProfile newProfile)
    {
        gameManager.gameSettingsProfile = newProfile;
    }
}
