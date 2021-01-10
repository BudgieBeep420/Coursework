using UnityEngine;

namespace Actual_Game_Files.Scripts.UI
{
    public abstract class BaseMenu : MonoBehaviour
    {
        protected abstract GameObject[] PanelArray { get; set; }
        protected abstract AudioManager AudioManager { get; set; }
        
        protected abstract AudioSource ThisAudioSource { get; set; }

        private enum Panels
        {
            Main,
            Options,
            Video,
            Audio,
            Game
        }
        
        public void ToMainPanel()
        {
            foreach (var panel in PanelArray) panel.SetActive(false);
            PanelArray[(int) Panels.Main].SetActive(true);
        }

        public void ToOptionPanel()
        {
            foreach (var panel in PanelArray) panel.SetActive(false);
            PanelArray[(int) Panels.Options].SetActive(true);
        }

        public void ToVideoPanel()
        {
            foreach (var panel in PanelArray) panel.SetActive(false);
            PanelArray[(int) Panels.Video].SetActive(true);
        }
    
        public void ToAudioPanel()
        {
            foreach (var panel in PanelArray) panel.SetActive(false);
            PanelArray[(int) Panels.Audio].SetActive(true);
        }
    
        public void ToGamePanel()
        {
            foreach (var panel in PanelArray) panel.SetActive(false);
            PanelArray[(int) Panels.Game].SetActive(true);
        }

        public void PlayHoverSound()
        {
            AudioManager.Play("ButtonHoverSound", ThisAudioSource);
        }

        public void PlayClickSound()
        {
            AudioManager.Play("ButtonClickSound", ThisAudioSource);
        }
    }
}