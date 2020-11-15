using System;
using System.Collections;
using System.Collections.Generic;
using Actual_Game_Files.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PistolScript : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject shotBullet;
    [SerializeField] private GameObject bulletCasing;
    [SerializeField] private GameObject whereCasingSpawns;
    [SerializeField] private GameObject endOfBarrel;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private AudioManager audioManager;
    [Space]
    
    [Header("Game Settings")]
    [SerializeField] private int numberOfBullets;
    [SerializeField] private float weaponInaccuracyInDegrees;
    [SerializeField] private float weaponShootingCooldown;
    [Space]
    
    [Header("SoundNames")]
    [SerializeField] private string pistolShotSoundName;
    [Space]
    
    private static readonly int HasShot = Animator.StringToHash("HasShot");
    private const float CasingRotation = 20f;
    private static Transform _playerTransform;
    private AudioSource _thisAudioSource;

    public bool canSeePlayer;

    private void Awake()
    {
        _thisAudioSource = gameObject.GetComponent<AudioSource>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }
    
    public void Shoot()
    {
        StartCoroutine(ShootingCooldown(weaponShootingCooldown));
        if (_playerTransform == null || !canSeePlayer) return;
        
        if (numberOfBullets == 0)
        {
            audioManager.Play("OutOfAmmoClick", _thisAudioSource);
            return;
        }
        
        
        numberOfBullets--;
        weaponAnimator.SetBool(HasShot, true);
        audioManager.Play(pistolShotSoundName, _thisAudioSource);

        Instantiate(shotBullet, endOfBarrel.transform.position,
            GenerateRandomBulletRotation(endOfBarrel.transform.rotation, weaponInaccuracyInDegrees));

        SpawnBulletCasing();
    }
    
    private IEnumerator ShootingCooldown(float time)
    {
        yield return new WaitForSeconds(Random.Range(0.3f * time, time));
        Shoot();
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
    
    private void SpawnBulletCasing()
    {
        var casing = Instantiate(bulletCasing);
        casing.transform.position = whereCasingSpawns.transform.position;
        casing.transform.rotation =
            GenerateRandomCasingRotation(whereCasingSpawns.transform.rotation, CasingRotation);
    }
    
    public void PlayTakingOutSound()
    {}
}
