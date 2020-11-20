using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        protected bool CanShoot { get; set; }
        private bool IsPuttingAway { get; set; }
        private static readonly int HasShot = Animator.StringToHash("HasShot");
        private static readonly int IsReloading = Animator.StringToHash("IsReloading");
        protected bool CanReload { get; set; }

        private const float CasingRotation = 20;
        private const float ShotgunStartingOffset = 0.1f;
        private const int NumberOfShotgunPellets = 20;
        private int _currentLoadedMag;
        private Color _greyColor = new Color(0.624f, 0.624f, 0.624f);
        
        private void Start()
        {
            UpdateMagazineAndAmmoCount(_currentLoadedMag);
        }

        private void OnEnable()
        {
            IsPuttingAway = false;
            UpdateMagazineAndAmmoCount(_currentLoadedMag);
        }

        protected void ShootingWeaponCheck()
        {
            var currentMagAmmo = MagazineArray[_currentLoadedMag].numOfBulletsInMag;
            
            if(currentMagAmmo == 0 && Input.GetKeyDown(KeyCode.Mouse0)) AudioManager.Play("OutOfAmmoClick", ThisAudioSource);
            if (IsPuttingAway)
            {
                CanShoot = false;
            }

            if (IsWeaponFullyAutomatic)
            {
                if (Input.GetKey(KeyCode.Mouse0) && CanShoot && currentMagAmmo != 0 && Math.Abs(Time.timeScale) > 0.15) ShootWeapon(currentMagAmmo);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && CanShoot && currentMagAmmo != 0 && Math.Abs(Time.timeScale) > 0.15) ShootWeapon(currentMagAmmo);
            }
        }

        private void ShootWeapon(int currentMagAmmo)
        {
            WeaponAnimator.SetBool(HasShot, true);
            AudioManager.Play(WeaponShotSoundName, ThisAudioSource);
            MagazineArray[_currentLoadedMag].numOfBulletsInMag--;
            currentMagAmmo--;
            UpdateCounterColour(AmmoText, currentMagAmmo, WeaponMagCapacity);
                
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
        
        private static Quaternion GenerateRandomBulletRotation(Quaternion initialRotation, float inaccuracy)
        {
            var randomX = Random.Range(-inaccuracy, inaccuracy) + 0.75;
            var randomY = Random.Range(-inaccuracy, inaccuracy) - 0.75;
            var randomZ = Random.Range(-inaccuracy, inaccuracy);

            return Quaternion.Euler((float)randomX, (float)randomY, randomZ) * initialRotation;
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
            /*CanReload = false;*/
            yield return new WaitForSeconds(time);
            CanShoot = true;
            CanReload = true;
        }

        private IEnumerator ReloadCooldown(float time)
        {
            CanReload = false;
            CanShoot = false;
            yield return new WaitForSeconds(time);
            CanReload = true;
            CanShoot = true;
        }

        
        // This is weird and doesnt work for some reason
        private void SpawnBulletCasing()
        {
            var casing = Instantiate(BulletCasing);
            casing.transform.position = WhereCasingSpawns.transform.position;
            casing.transform.rotation =
                GenerateRandomCasingRotation(WhereCasingSpawns.transform.rotation, CasingRotation);
        }

        private void Reload(float weaponReloadTime)
        {
            if (WeaponAnimator.GetBool(IsReloading) || !CanReload || !CanShoot) return;
            
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
            
            UpdateCounterColour(AmmoText, MagazineArray[magReloaded].numOfBulletsInMag, WeaponMagCapacity);
            
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

        private static void UpdateCounterColour(Text text, int numOfBullets, int capacity)
        {
            text.text = "Ammo: " + numOfBullets + "/ " + capacity;
            text.color = numOfBullets < capacity / 2 ? Color.yellow : new Color(0.624f, 0.624f, 0.624f);
            if (numOfBullets == 0) text.color = Color.red;
        }

        public void WeaponCantShoot()
        {
            CanShoot = false;
        }

        public void WeaponCanShoot()
        {
            CanShoot = true;
        }

        public void PuttingAway()
        {
            IsPuttingAway = true;
        }
    }
}

