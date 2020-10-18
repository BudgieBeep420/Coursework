using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionMenuScript : MonoBehaviour
{

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("FirstRoomScene");
    }
}
