using UnityEngine;

namespace Actual_Game_Files.Scripts
{
    [System.Serializable]
    public class Sound
    {
        [SerializeField] public string soundName;
        [SerializeField] public AudioSource audioSource;
        [SerializeField] [Range(0f, 1f)] private float volume;
        [SerializeField] [Range(.1f, 3f)] private float pitch;
    }
}
