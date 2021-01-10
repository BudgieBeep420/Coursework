using System;
using UnityEngine;

namespace Actual_Game_Files.Scripts
{
    [Serializable]
    public class Sound
    {
        [SerializeField] public string soundName;
        [SerializeField] public AudioClip audioClip;
        [SerializeField] public AudioSource audioSource;
        [SerializeField] public bool needsPlayOneHit;
        [SerializeField] [Range(0f, 1f)] public float volume;
        [SerializeField] [Range(.1f, 3f)] public float pitch;
    }
}
