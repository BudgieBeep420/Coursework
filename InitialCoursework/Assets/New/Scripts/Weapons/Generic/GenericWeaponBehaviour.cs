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
        /* These are all inherited by the classes for each weapon, then assigned a value
            by those classes (i.e. ShotBullet for the pistol would be set to a prefab
            of the pistol bullet.*/
        
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

        
        /* These are affected often by the animations, and Ienumerables */
        
        protected bool CanShoot { get; set; }
        private bool IsPuttingAway { get; set; }
        private static readonly int HasShot = Animator.StringToHash("HasShot");
        private static readonly int IsReloading = Animator.StringToHash("IsReloading");
        protected bool CanReload { get; set; }
        
        /* Some starting constants */

        private const float CasingRotation = 20;
        private const float ShotgunStartingOffset = 0.1f;
        private const int NumberOfShotgunPellets = 10;
        private int _currentLoadedMag;
        private Color _greyColor = new Color(0.624f, 0.624f, 0.624f);
        
        /* This makes sure the UI is up to date when the game is started */
        private void Start()
        {
            UpdateMagazineAndAmmoCount(_currentLoadedMag);
        }
        
        /* This makes sure that when the gun is taken out (when it is enabled), the UI shows the correct
            info for it.*/
        private void OnEnable()
        {
            IsPuttingAway = false;
            UpdateMagazineAndAmmoCount(_currentLoadedMag);
        }

        /* This is the main shooting method that is called on each frame a gun is enabled in */
        protected void ShootingWeaponCheck()
        {
            var currentMagAmmo = MagazineArray[_currentLoadedMag].numOfBulletsInMag;
            
            /* This simply plays a click if the player is out of ammo, like a normal gun */
            if(currentMagAmmo == 0 && Input.GetKeyDown(KeyCode.Mouse0)) AudioManager.Play("OutOfAmmoClick", ThisAudioSource);
            
            /* This makes sure that the player can't shoot while they are putting away the weapon */
            if (IsPuttingAway)
            {
                CanShoot = false;
            }

            /* This deals with the functionality of the Rifle */
            if (IsWeaponFullyAutomatic)
            {
                if (Input.GetKey(KeyCode.Mouse0) && CanShoot && currentMagAmmo != 0 && Math.Abs(Time.timeScale) > 0.15) ShootWeapon(currentMagAmmo);
            }
            else
            {
                /* This is ran if the gun isn't fully automatic ( it only shoots one shot at a time) */
                if (Input.GetKeyDown(KeyCode.Mouse0) && CanShoot && currentMagAmmo != 0 && Math.Abs(Time.timeScale) > 0.15) ShootWeapon(currentMagAmmo);
            }
        }

        /* This is the function alled when it is decided the player can shoot */
        private void ShootWeapon(int currentMagAmmo)
        {
            /* This plays the shooting animation */
            WeaponAnimator.SetBool(HasShot, true);
            
            /* This plays the shooting sound */
            AudioManager.Play(WeaponShotSoundName, ThisAudioSource);
            
            /* This decrements the number of bullets in the current mag */
            MagazineArray[_currentLoadedMag].numOfBulletsInMag--;
            currentMagAmmo--;
            
            /* This updates the UI with the new ammo count */
            UpdateCounterColour(AmmoText, currentMagAmmo, WeaponMagCapacity);
                
            /* This spawns a single bullet with random inaccuracy at the end of the barrel */
            if (!IsWeaponShotgun)
            {
                Instantiate(ShotBullet, EndOfBarrel.transform.position,
                    GenerateRandomBulletRotation(EndOfBarrel.transform.rotation, WeaponInaccuracyInDegrees))
                    .GetComponent<NewPistolBulletBehaviour>().IsBulletOfPlayer = true;
            }
            /* This spawns multiple bullets, like how pellets of a shotgun are produced, it is ran
                when the gun is shoot and the gun is marked as a shotgun*/
            else
            {
                for (var i = 0; i <= NumberOfShotgunPellets; i++)
                    Instantiate(ShotBullet,
                        GenerateRandomBulletStartingPosition(EndOfBarrel.transform.position, ShotgunStartingOffset),
                        GenerateRandomBulletRotation(EndOfBarrel.transform.rotation, WeaponInaccuracyInDegrees))
                        .GetComponent<ShotgunBulletBehaviour>().IsBulletOfPlayer = true;
            }
                
            /* This makes sure the player has to wait in between shots */
            StartCoroutine(ShootingCooldown(TimeBetweenShots));
            
            SpawnBulletCasing();
        }
        
        
        /* These introduce a certain amount of randomness to aspects of the weapon to make them more
                realistic*/
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

        /* This is called when the player has just shot */
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
        
        private void SpawnBulletCasing()
        {
            var casing = Instantiate(BulletCasing);
            casing.transform.position = WhereCasingSpawns.transform.position;
            casing.transform.rotation =
                GenerateRandomCasingRotation(WhereCasingSpawns.transform.rotation, CasingRotation);
        }

        /* Called when the player successfully reloads (checked every frame by this base class */
        private void Reload(float weaponReloadTime)
        {
            /* If the player is already reloading, or in a current animation, it won't reload */
            if (WeaponAnimator.GetBool(IsReloading) || !CanReload || !CanShoot) return;
            
            /* Sets reload sound and animation, as well as sets a cooldown to reloading off */
            WeaponAnimator.SetBool(IsReloading, true);
            AudioManager.Play(WeaponReloadSoundName, ThisAudioSource);
            StartCoroutine(ReloadCooldown(weaponReloadTime));
            
            /* This is the actual algorithm for reloading. It loads all of the magazines into a list, and
                determines the one with the largest current ammo left, then reloads that one.*/
            var desiredMagIndex = 0;
            var currentLargest = 0;
            
            foreach(var magazine in MagazineArray)
            {
                if (magazine.numOfBulletsInMag <= currentLargest) continue;
                desiredMagIndex = Array.IndexOf(MagazineArray, magazine);
                currentLargest = magazine.numOfBulletsInMag;
            }
            
            /* Updates Ui */
            UpdateMagazineAndAmmoCount(desiredMagIndex);
            _currentLoadedMag = desiredMagIndex;
        }

        /* Makes sure the player only reloads if they press R and there is still ammo left */
        protected void CheckReload()
        {
            if (!Input.GetKeyDown(KeyCode.R) || MagazineArray.All(mag => mag.numOfBulletsInMag == 0)) return;
            Reload(WeaponReloadTime);
        }

        /* Updates UI for the current weapons and their data (mags left, etc.) */
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
        
        private static void UpdateCounterColour(Text text, int numOfBullets, int capacity)
        {
            text.text = "Ammo: " + numOfBullets + "/ " + capacity;
            text.color = numOfBullets < capacity / 2 ? Color.yellow : new Color(0.624f, 0.624f, 0.624f);
            if (numOfBullets == 0) text.color = Color.red;
        }
        
        /* Called by animator etc when the weapon is being put away */
        /* The rest of these are just used as functions to change boolean values by the animator / code */
        
        protected void DisableWeapon()
        {
            gameObject.SetActive(false);
        }
        
        private void PlayTakingOutSound()
        {
            if(ThisAudioSource != null) AudioManager.Play("WeaponUnholstering", ThisAudioSource);
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
