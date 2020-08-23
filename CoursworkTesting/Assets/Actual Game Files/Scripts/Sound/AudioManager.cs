using System;
using UnityEngine;

namespace Actual_Game_Files.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] sounds;
        
        public void Play(string nameOfSound)
        {
            var _sound = Array.Find(sounds,sound => sound.soundName == nameOfSound);
            _sound.audioSource.Play();
        }
    }
}
