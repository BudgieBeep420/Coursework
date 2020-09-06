using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Analytics;

namespace Actual_Game_Files.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] public Sound[] sounds;

        private void Start()
        {
            AudioListener.volume = 0.5f;
        }
        
        public void Play(string nameOfSound, AudioSource desiredAudioSource)
        {
            if (desiredAudioSource == null)
            {
                Debug.Log("Can't Find desire audio source!");
                return;
            }
            
            var _sound = Array.Find(sounds,sound => sound.soundName == nameOfSound);
            _sound.audioSource = desiredAudioSource;

            if (_sound.audioSource.clip != _sound.audioClip)
                _sound.audioSource.clip = _sound.audioClip;
            
            SetVolumeAndPitch(_sound, desiredAudioSource);
            _sound.audioSource.Play();
        }

        private static void SetVolumeAndPitch(Sound sound, AudioSource audioSource)
        {
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
        }
    }
}
