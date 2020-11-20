using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using Actual_Game_Files.Scripts.Serializable;
using UnityEngine;
using UnityEngine.UI;


public class VideoPanel : MonoBehaviour
{
    private Resolution[] _resolutions;

    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private GameManagerScript gameManagerScript;

    private VideoSettingsProfile _videoSettingsProfile;
    
    private string _videoSettingsDirectory;

    private void OnEnable()
    {
        _videoSettingsDirectory = Application.dataPath + @"\Settings\VideoSettings.json";
        _videoSettingsProfile = gameManagerScript.videoSettingsProfile;
        InitializeVideoPanel();
    }

    private void Awake()
    {
        _resolutions = Screen.resolutions;
        var resolutionOptions = _resolutions.Select(resolution => resolution.width + " x " + resolution.height).ToList();

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
    }

    public void UpdateResolution(int resolutionIndex)
    {
        Debug.Log(resolutionIndex);
        Screen.SetResolution(_resolutions[resolutionIndex].width, _resolutions[resolutionIndex].height, _videoSettingsProfile.isFullscreen);
        _videoSettingsProfile.resolutionIndex = resolutionIndex;
        gameManagerScript.videoSettingsProfile.resolutionIndex = resolutionIndex;
    }

    public void UpdateQuality(int qualityIndex)
    {
        Debug.Log("Quality index: " + qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
        _videoSettingsProfile.qualityIndex = qualityIndex;
        gameManagerScript.videoSettingsProfile.qualityIndex = qualityIndex;
    }

    public void UpdateFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        _videoSettingsProfile.isFullscreen = isFullscreen;
        gameManagerScript.videoSettingsProfile.isFullscreen = isFullscreen;
    }

    private void InitializeVideoPanel()
    {
        resolutionDropdown.value = _videoSettingsProfile.resolutionIndex;
        qualityDropdown.value = _videoSettingsProfile.qualityIndex;
        fullscreenToggle.isOn = _videoSettingsProfile.isFullscreen;
    }

    public void WriteVideoSettings()
    {
        File.WriteAllText(_videoSettingsDirectory, JsonUtility.ToJson(_videoSettingsProfile));
    }
}
