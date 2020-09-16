using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Actual_Game_Files.Scripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioPanel : MonoBehaviour
{
    [Header("Text")] 
    [SerializeField] private Text globalVolumeText;
    [SerializeField] private Text pitchText;
    [SerializeField] private Text musicVolumeText;
    [Space]
    
    [Header("Sliders")] 
    [SerializeField] private Slider globalVolumeSlider;
    [SerializeField] private Slider pitchSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [Space] 
    
    [SerializeField] private AudioManager audioManager;


    private void OnEnable()
    {
        InitializeAudioSliderSettings();
    }

    private void InitializeAudioSliderSettings()
    {
        var audioProfile = audioManager.audioSettingsProfile;
        globalVolumeSlider.value = audioProfile.globalVolume;
        pitchSlider.value = audioProfile.pitch / 3;
        musicVolumeSlider.value = audioProfile.musicVolume;
    }
    
    public void UpdateGlobalVolText(float value)
    {
        globalVolumeText.text = Math.Round(value * 100).ToString(CultureInfo.InvariantCulture) + "%";
    }
    
    public void UpdatePitchText(float value)
    {
        pitchText.text = Math.Round(value * 100 * 3).ToString(CultureInfo.InvariantCulture) + "%";
    }
    
    public void UpdateMusicVolText(float value)
    {
        musicVolumeText.text = Math.Round(value * 100).ToString(CultureInfo.InvariantCulture) + "%";
    }


}
