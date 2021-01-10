using System.Collections;
using Actual_Game_Files.Scripts;
using UnityEngine;


public class TutorialScript : MonoBehaviour
{
    [SerializeField] private AudioSource tutorialAudioSource;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject subtitlePanel;
    [SerializeField] private LevelManagerScript levelManagerScript;

    private const string Tut1 = "TutorialOpening";
    private const string Tut2 = "TutorialTable";
    private const string Tut3 = "TutorialHUD";

    [SerializeField] private GameObject[] subtitleArray;
    
    private void Start()
    {
        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(5f);
        
        subtitleArray[0].SetActive(true);
        audioManager.Play(Tut1, tutorialAudioSource);
        yield return new WaitForSeconds(23f);
        
        subtitleArray[0].SetActive(false);
        subtitleArray[1].SetActive(true);
        audioManager.Play(Tut3, tutorialAudioSource);
        yield return new WaitForSeconds(24f);
        
        subtitleArray[1].SetActive(false);
        subtitleArray[2].SetActive(true);
        audioManager.Play(Tut2, tutorialAudioSource);
        yield return new WaitForSeconds(15f);
        
        levelManagerScript.FinishedTutorial();
        subtitlePanel.SetActive(false);
    }
}
