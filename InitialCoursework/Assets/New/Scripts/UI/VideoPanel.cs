using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.CompilerServices;
using Actual_Game_Files.Scripts.Serializable;
using UnityEngine;
using UnityEngine.UI;


public class VideoPanel : MonoBehaviour
{
    private Resolution[] _resolutions;

    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider fieldOfViewSlider;
    [SerializeField] private Text fieldOfViewText;
    [SerializeField] private GameManagerScript gameManagerScript;
    [SerializeField] private Camera playerCamera;

    private VideoSettingsProfile _videoSettingsProfile;
    private string _videoSettingsDirectory;
    private float fovPercentageFactor = 0.4f;

    private void OnEnable()
    {
        _videoSettingsDirectory = Directory.GetCurrentDirectory() + @"\Settings\VideoSettings.json";
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

    public void UpdateFOV(float fovPercentage)
    {
        // For this, we use fov = 0 for 70, fov = 50 for 90, and fov = 100 or 110
        // so 1 percent = 0.4 degrees, plus 70 degs to start.

        playerCamera.fieldOfView = PercentageToActual(fovPercentage);
        _videoSettingsProfile.fieldOfView = (int)fovPercentage;
        gameManagerScript.videoSettingsProfile.fieldOfView = (int)fovPercentage;
    }
    
    public void UpdateFovText(float value)
    {
        fieldOfViewText.text = PercentageToActual(value).ToString(CultureInfo.InvariantCulture);
    }

    private float PercentageToActual(float value)
    {
        return value * fovPercentageFactor + 70;
    }

    private void InitializeVideoPanel()
    {
        resolutionDropdown.value = _videoSettingsProfile.resolutionIndex;
        qualityDropdown.value = _videoSettingsProfile.qualityIndex;
        fullscreenToggle.isOn = _videoSettingsProfile.isFullscreen;
        fieldOfViewSlider.value = _videoSettingsProfile.fieldOfView;
    }

    public void WriteVideoSettings()
    {
        File.WriteAllText(_videoSettingsDirectory, JsonUtility.ToJson(_videoSettingsProfile));
    }
}
