using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Actual_Game_Files.Scripts
{
    public abstract class GenericWeaponBehaviour : MonoBehaviour
    {
        protected abstract GameObject ShotBullet { get; set; }
        protected abstract GameObject BulletCasing { get; set; }
        protected abstract GameObject WhereCasingSpawns { get; set; }
        protected abstract GameObject EndOfBarrel { get; set; }
        protected abstract Animator WeaponAnimator { get; set; }
        protected abstract AudioManager AudioManager { get; set; }
        protected abstract Text AmmoText { get; set; }
        protected abstract Text MagazineText { get; set; }

        protected abstract float TimeBetweenShots { get; set; }
        protected abstract float WeaponInaccuracyInDegrees { get; set; }
        protected abstract Magazine[] MagazineArray { get; set; }
        protected abstract int WeaponMagCapacity { get; set; }


        protected abstract string WeaponShotSoundName { get; set; }

        protected abstract bool CanShoot { get; set; }
        private static readonly int HasShot = Animator.StringToHash("HasShot");
        private static readonly int IsReloading = Animator.StringToHash("IsReloading");
        protected abstract bool CanReload { get; set; }

        private const float CasingRotation = 20;
        private int _currentLoadedMag;
        private AudioSource _thisAudioSource;


        private void Start()
        {
            _thisAudioSource = gameObject.GetComponent<AudioSource>();
            UpdateMagazineAndAmmoCount(0);
        }

        protected void ShootingWeaponCheck()
        {
            var currentMagAmmo = MagazineArray[_currentLoadedMag].numOfBulletsInMag;
            
            if(currentMagAmmo == 0 && Input.GetKeyDown(KeyCode.Mouse0)) AudioManager.Play("OutOfAmmoClick", _thisAudioSource);
                
            if (!Input.GetKeyDown(KeyCode.Mouse0) || !CanShoot || currentMagAmmo == 0) return;
            
            WeaponAnimator.SetBool(HasShot, true);
            AudioManager.Play(WeaponShotSoundName, _thisAudioSource);
            MagazineArray[_currentLoadedMag].numOfBulletsInMag -= 1;
            AmmoText.text = "Ammo: " + MagazineArray[_currentLoadedMag].numOfBulletsInMag + "/ " + WeaponMagCapacity;
            Instantiate(ShotBullet, EndOfBarrel.transform.position,
                GenerateRandomBulletRotation(EndOfBarrel.transform.rotation, WeaponInaccuracyInDegrees));
            StartCoroutine(ShootingCooldown(TimeBetweenShots));
            SpawnBulletCasing();
        }

        private static Quaternion GenerateRandomBulletRotation(Quaternion initialRotation, float inaccuracy)
        {
            var randomX = Random.Range(-inaccuracy, inaccuracy) + 0.75;
            var randomY = Random.Range(-inaccuracy, inaccuracy) - 0.75;
            var randomZ = Random.Range(-inaccuracy, inaccuracy);

            return Quaternion.Euler((float)randomX, (float)randomY, (float)randomZ) * initialRotation;
        }
        
        private static Quaternion GenerateRandomCasingRotation(Quaternion initialRotation, float inaccuracy)
        {
            var randomX = Random.Range(-inaccuracy, inaccuracy);
            var randomY = Random.Range(-inaccuracy, inaccuracy);
            var randomZ = Random.Range(-inaccuracy, inaccuracy);

            return Quaternion.Euler(randomX, randomY, randomZ) * initialRotation;
        }

        private IEnumerator ShootingCooldown(float time)
        {
            CanShoot = false;
            yield return new WaitForSeconds(time);
            CanShoot = true;
        }

        private IEnumerator ReloadCooldown(float time)
        {
            CanReload = false;
            CanShoot = false;
            yield return new WaitForSeconds(time);
            CanReload = true;
            CanShoot = true;
        }

        private void SpawnBulletCasing()
        {
            var casing = Instantiate(BulletCasing);
            casing.transform.position = WhereCasingSpawns.transform.position;
            casing.transform.rotation =
                GenerateRandomCasingRotation(WhereCasingSpawns.transform.rotation, CasingRotation);
        }

        private void Reload()
        {
            if (WeaponAnimator.GetBool(IsReloading) || !CanReload) return;
            
            WeaponAnimator.SetBool(IsReloading, true);
            AudioManager.Play("PistolReloadSound1", _thisAudioSource);
            StartCoroutine(ReloadCooldown(2));
            var desiredMagIndex = 0;
            var currentLargest = 0;
            
            foreach(var magazine in MagazineArray)
            {
                if (magazine.numOfBulletsInMag <= currentLargest) continue;
                desiredMagIndex = Array.IndexOf(MagazineArray, magazine);
                currentLargest = magazine.numOfBulletsInMag;
            }
            
            UpdateMagazineAndAmmoCount(desiredMagIndex);
            _currentLoadedMag = desiredMagIndex;
        }

        protected void CheckReload()
        {
            if (!Input.GetKeyDown(KeyCode.R)) return;
            Reload();
        }

        private void UpdateMagazineAndAmmoCount(int magReloaded)
        {
            var tempList = new List<int>();
            
            AmmoText.text = "Ammo: " + MagazineArray[magReloaded].numOfBulletsInMag + "/ " + WeaponMagCapacity;
            
            foreach (var magazine in MagazineArray)
            {
                if (Array.IndexOf(MagazineArray, magazine) == magReloaded || magazine.numOfBulletsInMag == 0) continue;
                tempList.Add(magazine.numOfBulletsInMag);
            }
            
            MagazineText.text = "Magazines: " + string.Join(" - ", tempList);
        }
    }

}

