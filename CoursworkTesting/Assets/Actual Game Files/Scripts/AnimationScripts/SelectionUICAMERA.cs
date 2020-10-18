using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class SelectionUICAMERA : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    
    public void EnableUI()
    {
        canvas.SetActive(true);
    }
}
