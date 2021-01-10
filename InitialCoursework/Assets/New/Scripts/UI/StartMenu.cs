using UnityEngine;
using UnityEngine.SceneManagement;

namespace Actual_Game_Files.Scripts.UI
{
    public class StartMenu : BaseMenu
    {
        [Header("Panels")] 
        [SerializeField] private GameObject[] panels;
        [Space]
        
        [SerializeField] private AudioManager audioManager;

        protected override GameObject[] PanelArray { get; set; }
        protected override AudioManager AudioManager { get; set; }
        protected override AudioSource ThisAudioSource { get; set; }

        private void Awake()
        {
            PanelArray = panels;
            AudioManager = audioManager;
            ThisAudioSource = gameObject.GetComponent<AudioSource>();
        }

        public void CloseGame()
        {
            Application.Quit();
        }

        public void SelectionMenu()
        {
            SceneManager.LoadScene("LevelSelectionScene");
        }
    }
}