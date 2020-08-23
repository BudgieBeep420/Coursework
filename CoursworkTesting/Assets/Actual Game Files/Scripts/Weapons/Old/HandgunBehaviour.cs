using System;
using System.Collections;
using Actual_Game_Files.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandgunBehaviour : MonoBehaviour
{
        [Header("Objects")]
        [SerializeField] private GameObject shotBullet;
        [SerializeField] private GameObject bulletCasing;
        [SerializeField] private GameObject endOfBarrel;
        [SerializeField] private Animator pistolAnimator;
        [SerializeField] private GameObject bulletBin;
        [SerializeField] private AudioManager audioManager;
        [Space]
            
        [Header("Game Settings")]
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float timeBetweenShots;
        [SerializeField] private float pistolInaccuracyInDegrees = 10f;
        [Space]

        [Header("SoundNames")]
        [SerializeField] private string pistolShotSoundName;
        [Space]
        
        private bool canShoot = true;
        private static readonly int HasShot = Animator.StringToHash("HasShot");
        private Rigidbody bulletRb;

        private void Awake()
        {
            bulletRb = shotBullet.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            OnShootingPistol();
        }

        private void OnShootingPistol()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse0) || !canShoot) return;
            
            pistolAnimator.SetBool(HasShot, true);
            audioManager.Play(pistolShotSoundName);
            var bullet = Instantiate(shotBullet, endOfBarrel.transform.position, GenerateRandomBulletRotation(endOfBarrel.transform.rotation, pistolInaccuracyInDegrees));
            bulletRb.velocity = bullet.transform.up * (bulletSpeed * Time.deltaTime);
            bullet.transform.SetParent(bulletBin.transform);
            StartCoroutine(WaitForTime(timeBetweenShots));
        }

        private static Quaternion GenerateRandomBulletRotation(Quaternion initialRotation, float inaccuracy)
        {
            var randomX = Random.Range(-inaccuracy, inaccuracy);
            var randomY = Random.Range(-inaccuracy, inaccuracy);
            var randomZ = Random.Range(-inaccuracy, inaccuracy) + 1;
            
            var newRotation = Quaternion.Euler(randomX, randomY, randomZ) * initialRotation;
            
            return newRotation;
        }

        private IEnumerator WaitForTime(float time)
        {
            canShoot = false;
            yield return new WaitForSeconds(time);
            canShoot = true;
        }

}
