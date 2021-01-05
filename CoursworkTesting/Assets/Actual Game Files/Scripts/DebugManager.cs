using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Actual_Game_Files.Scripts;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private DebugScript debugScript;
    
    private void Update()
    {
        if (Input.GetKeyDown("`") && debugScript.enabled) debugScript.enabled = false;
        else if (Input.GetKeyDown("`") && !debugScript.enabled) debugScript.enabled = true;
    }
}
