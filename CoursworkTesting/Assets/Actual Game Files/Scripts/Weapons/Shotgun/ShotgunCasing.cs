using System.Collections;
using UnityEngine;

namespace Actual_Game_Files.Scripts.Weapons.Shotgun
{
    public class ShotgunCasing : CasingBehaviour
    {
        [SerializeField] private AudioSource thisAudioSource;
        
        protected override IEnumerator CasingSound()
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));

            audioManager.Play(Random.Range(0, 2) == 0 ? "ShellHittingFloor1" : "ShellHittingFloor2", thisAudioSource);
        }
    }
}