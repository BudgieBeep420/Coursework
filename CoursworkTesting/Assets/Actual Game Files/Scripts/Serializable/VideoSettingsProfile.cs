using System;

namespace Actual_Game_Files.Scripts.Serializable
{
    [Serializable]
    public class VideoSettingsProfile
    {
        public int qualityIndex;
        public int resolutionIndex;
        public bool isFullscreen;
        public int fieldOfView;
    }
}