using System;
using UnityEngine;

/* Marked as Serializable so it can be turned into instances inside the code */
/* Note: this does not inherit from MonoBehaviour, as it is not required for this code */
[Serializable]
public class AudioSettingsProfile
{
    /* These are all the values that I want to save into the JSON files for Audio */
    [Range(0f, 1f)] public float musicVolume = 1;
    [Range(0.1f, 3f)] public float pitch = 1;
    [Range(0f, 1f)] public float globalVolume = 1;
}
