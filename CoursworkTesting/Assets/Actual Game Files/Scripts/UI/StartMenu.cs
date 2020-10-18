using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Actual_Game_Files.Scripts.UI
{
    public class StartMenu : BaseMenu
    {
        [Header("Panels")] 
        [SerializeField] private GameObject[] panels;
        
        protected override GameObject[] PanelArray { get; set; }

        private void Awake()
        {
            PanelArray = panels;
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