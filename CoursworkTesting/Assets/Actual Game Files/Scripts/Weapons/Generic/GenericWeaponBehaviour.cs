using System.Collections;
using UnityEngine;

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
        protected abstract GameObject CasingsBin { get; set; }
        
        protected abstract float TimeBetweenShots { get; set; }
        protected abstract float WeaponInaccuracyInDegrees { get; set; }
        
        protected abstract string WeaponShotSoundName { get; set; }

        protected abstract bool CanShoot { get; set; }
        private static readonly int HasShot = Animator.StringToHash("HasShot");
        private const float CasingRotation = 20;
        
        protected Rigidbody casingRb;

        protected void ShootingWeaponCheck()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse0) || !CanShoot) return;

            WeaponAnimator.SetBool(HasShot, true);
            AudioManager.Play(WeaponShotSoundName);
            Instantiate(ShotBullet, EndOfBarrel.transform.position,
                GenerateRandomBulletRotation(EndOfBarrel.transform.rotation, WeaponInaccuracyInDegrees));
            StartCoroutine(WaitForTime(TimeBetweenShots));
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

        private IEnumerator WaitForTime(float time)
        {
            CanShoot = false;
            yield return new WaitForSeconds(time);
            CanShoot = true;
        }
        
        private void SpawnBulletCasing()
        {
            var casing = Instantiate(BulletCasing);
            casing.transform.position = WhereCasingSpawns.transform.position;
            casing.transform.rotation =
                GenerateRandomBulletRotation(WhereCasingSpawns.transform.rotation, CasingRotation);
        }
    }

}

