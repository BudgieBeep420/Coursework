using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Actual_Game_Files.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Sounds")]
        [SerializeField] public Sound[] sounds;
        [Space]
        
        [Header("GameObjects")]
        [SerializeField] public AudioSource musicSource;
        [SerializeField] private AudioSource backgroundSoundSource;
        [SerializeField] private Slider globalVolSlider;
        [SerializeField] private Slider pitchSlider;
        [SerializeField] private Slider musicVolSlider;
        [Space]


        public AudioSettingsProfile audioSettingsProfile;
        private string _audioSettingsDirectory;


        private void Awake()
        {
            _audioSettingsDirectory = Directory.GetCurrentDirectory() + @"\Settings\AudioSettings.json";
            audioSettingsProfile = JsonUtility.FromJson<AudioSettingsProfile>(File.ReadAllText(_audioSettingsDirectory));
            PlayMusic();
            PlayBackGroundMusic();
        }

        public void WriteAudioSettings()
        {
            /* This does 3 things: updates the AudioSettingsProfile inside this script,
                as well as writing this new file to the appropriate directory, then 
                it resets the music and background sounds as they don't automatically
                update*/
            
            audioSettingsProfile.globalVolume = globalVolSlider.value;
            audioSettingsProfile.pitch = pitchSlider.value * 3;
            audioSettingsProfile.musicVolume = musicVolSlider.value;
            File.WriteAllText(_audioSettingsDirectory, JsonUtility.ToJson(audioSettingsProfile));
            PlayMusic();
            PlayBackGroundMusic();
        }

        public void Play(string nameOfSound, AudioSource desiredAudioSource)
        {
            // ReSharper disable once InconsistentNaming
            /* This part is called whenever another script wants to play a sound
                First, it queries  the array using LINQ to find the appropriate name
                and then plays it at the desired place.*/
            
            var _sound = Array.Find(sounds,sound => sound.soundName == nameOfSound);
            _sound.audioSource = desiredAudioSource;

            if (_sound.audioSource.clip != _sound.audioClip)
                _sound.audioSource.clip = _sound.audioClip;
            
            SetVolumeAndPitch(_sound, desiredAudioSource);
            
            if(_sound.needsPlayOneHit) _sound.audioSource.PlayOneShot(_sound.audioClip);
            else _sound.audioSource.Play();
        }

        private void SetVolumeAndPitch(Sound sound, AudioSource audioSource)
        {
            /* This function is called every time a sound is played. It applies the users
                predefined audio settings to the sound clip and sound source*/
            
            audioSource.volume = sound.volume * audioSettingsProfile.globalVolume;

            if (sound.soundName == "InGameMusic")
                audioSource.volume *= audioSettingsProfile.musicVolume;
            
            audioSource.pitch = sound.pitch * audioSettingsProfile.pitch;
        }

        private void PlayMusic()
        {
            Play("InGameMusic", musicSource);
        }

        private void PlayBackGroundMusic()
        {
            Play("BackGroundNoise", backgroundSoundSource);
        }
    }
}
