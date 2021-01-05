using UnityEngine;

namespace Actual_Game_Files.Scripts
{
    public class DebugScript : MonoBehaviour
    {
        private static string log = "";
        private string output;
        private string stack;
     
        private void OnEnable()
        {
            Application.logMessageReceived += Log;
        }
     
        private void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }
     
        private void Log(string logString, string stackTrace, LogType type)
        {
            output = logString;
            stack = stackTrace;
            log = output + "\n" + log;
            if (log.Length > 5000)
            {
                log = log.Substring(0, 4000);
            }
        }
     
        private void OnGUI()
        {
            //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            {
                log = GUI.TextArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10), log);
            }
        }
    }
}