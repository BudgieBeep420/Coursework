using System;
using UnityEngine;

[Serializable]
public class AudioSettingsProfile
{
    [Range(0f, 1f)] public float musicVolume = 1;
    [Range(0.1f, 3f)] public float pitch = 1;
    [Range(0f, 1f)] public float globalVolume = 1;
}
