using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        protected abstract string WeaponReloadSoundName { get; set; }
        protected abstract bool IsWeaponFullyAutomatic { get; set; }
        protected abstract bool IsWeaponShotgun { get; set; }
        protected abstract float WeaponReloadTime { get; set; }
        protected abstract AudioSource ThisAudioSource { get; set; }

        protected abstract bool CanShoot { get; set; }
        private static readonly int HasShot = Animator.StringToHash("HasShot");
        private static readonly int IsReloading = Animator.StringToHash("IsReloading");
        protected abstract bool CanReload { get; set; }

        private const float CasingRotation = 20;
        private const float ShotgunStartingOffset = 0.1f;
        private const int NumberOfShotgunPellets = 10;
        private int _currentLoadedMag;

        private void Start()
        {
            UpdateMagazineAndAmmoCount(_currentLoadedMag);
        }

        private void OnEnable()
        {
            UpdateMagazineAndAmmoCount(_currentLoadedMag);
        }

        protected void ShootingWeaponCheck()
        {
            var currentMagAmmo = MagazineArray[_currentLoadedMag].numOfBulletsInMag;
            
            if(currentMagAmmo == 0 && Input.GetKeyDown(KeyCode.Mouse0)) AudioManager.Play("OutOfAmmoClick", ThisAudioSource);
            
            if (Input.GetKeyDown(KeyCode.Mouse0) && CanShoot && currentMagAmmo != 0)
            {
                WeaponAnimator.SetBool(HasShot, true);
                AudioManager.Play(WeaponShotSoundName, ThisAudioSource);
                MagazineArray[_currentLoadedMag].numOfBulletsInMag -= 1;
                AmmoText.text = "Ammo: " + MagazineArray[_currentLoadedMag].numOfBulletsInMag + "/ " +
                                WeaponMagCapacity;
                if (!IsWeaponShotgun)
                {
                    Instantiate(ShotBullet, EndOfBarrel.transform.position,
                        GenerateRandomBulletRotation(EndOfBarrel.transform.rotation, WeaponInaccuracyInDegrees));
                }
                else
                {
                    for(var i = 0; i <= NumberOfShotgunPellets; i++)
                        Instantiate(ShotBullet, 
                            GenerateRandomBulletStartingPosition(EndOfBarrel.transform.position, ShotgunStartingOffset),
                            GenerateRandomBulletRotation(EndOfBarrel.transform.rotation, WeaponInaccuracyInDegrees));
                }
                
                StartCoroutine(ShootingCooldown(TimeBetweenShots));
                SpawnBulletCasing();
            }
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

        private static Vector3 GenerateRandomBulletStartingPosition(Vector3 startingPosition, float potentialOffset)
        {
            var randomX = Random.Range(-potentialOffset, potentialOffset);
            var randomY = Random.Range(-potentialOffset, potentialOffset);
            var randomZ = Random.Range(-potentialOffset, potentialOffset);
            
            return new Vector3(randomX, randomY, randomZ) + startingPosition;
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

        private void Reload(float weaponReloadTime)
        {
            if (WeaponAnimator.GetBool(IsReloading) || !CanReload) return;
            
            WeaponAnimator.SetBool(IsReloading, true);
            AudioManager.Play(WeaponReloadSoundName, ThisAudioSource);
            StartCoroutine(ReloadCooldown(weaponReloadTime));
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
            if (!Input.GetKeyDown(KeyCode.R) || MagazineArray.All(mag => mag.numOfBulletsInMag == 0)) return;
            Reload(WeaponReloadTime);
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

        protected void DisableWeapon()
        {
            gameObject.SetActive(false);
        }
        
        private void PlayTakingOutSound()
        {
            if(ThisAudioSource != null) AudioManager.Play("WeaponUnholstering", ThisAudioSource);
        }
    }

}

