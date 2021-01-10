using System.IO;
using Actual_Game_Files.Scripts.Serializable;
using UnityEngine;

public class CameraFOVScript : MonoBehaviour
{
    private void Start()
    {
        var videoSettingsDirectory = Directory.GetCurrentDirectory() + @"\Settings\VideoSettings.json";
        var videoSettingsProfile = JsonUtility.FromJson<VideoSettingsProfile>(File.ReadAllText(videoSettingsDirectory));
        gameObject.GetComponent<Camera>().fieldOfView = videoSettingsProfile.fieldOfView * 0.4f + 70;
        enabled = false;
    }
}
