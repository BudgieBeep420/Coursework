using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionMenuScript : MonoBehaviour
{
    /* These functions are called when their respective buttons are pressed. They enter the appropriate level*/
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void Mission1()
    {
        SceneManager.LoadScene("LevelOne");
    }
}
