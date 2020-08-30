using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actual_Game_Files.Scripts
{
    [Serializable]
    public class Sound
    {
        [SerializeField] public string soundName;
        [SerializeField] public AudioClip audioClip;
        [SerializeField] public AudioSource audioSource;
        [SerializeField] [Range(0f, 1f)] private float volume;
        [SerializeField] [Range(.1f, 3f)] private float pitch;
    }
}
