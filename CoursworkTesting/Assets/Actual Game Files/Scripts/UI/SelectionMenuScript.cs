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
        SceneManager.LoadScene("TutorialScene");
    }

    public void Mission1()
    {
        SceneManager.LoadScene("LevelOne");
    }
}
