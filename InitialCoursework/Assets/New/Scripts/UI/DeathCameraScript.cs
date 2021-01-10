using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCameraScript : MonoBehaviour
{
    [SerializeField] private GameObject deathCanvas;

    public void InstantiateDeathCanvas()
    {
        foreach(var obj in GameObject.FindGameObjectsWithTag("HUD")) obj.SetActive(false);
        Instantiate(deathCanvas);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
