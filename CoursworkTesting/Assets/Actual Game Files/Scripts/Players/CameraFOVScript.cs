using System.IO;
using Actual_Game_Files.Scripts.Serializable;
using UnityEngine;

public class CameraFOVScript : MonoBehaviour
{
    
    /* This initialises the FOV of the camera, as otherwise the camera would just deafault to the normal 80
        degree FOV, this sets it to the setting the player defined.*/
    private void Start()
    {
        var videoSettingsDirectory = Directory.GetCurrentDirectory() + @"\Settings\VideoSettings.json";
        var videoSettingsProfile = JsonUtility.FromJson<VideoSettingsProfile>(File.ReadAllText(videoSettingsDirectory));
        gameObject.GetComponent<Camera>().fieldOfView = videoSettingsProfile.fieldOfView * 0.4f + 70;
        enabled = false;
    }
}
