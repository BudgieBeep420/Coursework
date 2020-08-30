using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Analytics;

namespace Actual_Game_Files.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] public Sound[] sounds;
        
        public void Play(string nameOfSound, AudioSource desiredAudioSource)
        {
            if (desiredAudioSource == null)
            {
                Debug.Log("Can't Find desire audio source!");
                return;
            }
            
            var _sound = Array.Find(sounds,sound => sound.soundName == nameOfSound);

            if (_sound.audioSource.clip != _sound.audioClip)
            {
                _sound.audioSource.clip = _sound.audioClip;
            }

            _sound.audioSource = desiredAudioSource;
            _sound.audioSource.Play();
        }
        
    }
}
