using System;
using System.Collections;
using System.Collections.Generic;
using Actual_Game_Files.Scripts;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerBehaviourScript : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private GameObject cameraDeathHolder;
    [SerializeField] private Text healthText;
    [Space]
    
    [Header("Weapons")]
    [SerializeField] private GameObject[] weaponArray;
    [Space]
    
    [Header("WeaponGameObjects")]
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject shotgun;
    [Space]
    
    [Header("WeaponAnimators")]
    [SerializeField] private Animator knifeAnimator;
    [SerializeField] private Animator pistolAnimator;
    [SerializeField] private Animator rifleAnimator;
    [SerializeField] private Animator shotgunAnimator;
    [Space] 
    
    [Header("WeaponActivationImages")]
    [SerializeField] private Image[] weaponImages;
    [Space]
    
    [Header("Game Settings")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    private static readonly int PlayerHasDied = Animator.StringToHash("PlayerHasDied");
    private static readonly int IsPuttingAway = Animator.StringToHash("IsPuttingAway");

    private const KeyCode KnifeKey = KeyCode.Alpha1;
    private const KeyCode PistolKey = KeyCode.Alpha2;
    private const KeyCode RifleKey = KeyCode.Alpha3;
    private const KeyCode ShotgunKey = KeyCode.Alpha4;

    private GenericWeaponBehaviour _gwb;
    
    public enum Weapons
    {
        Knife,
        Pistol,
        Rifle,
        Shotgun
    }

    public Weapons? currentActiveWeapon = null;

    private void Start()
    {
        healthText.text = "Health: " + health + "/ " + maxHealth;
    }

    private void Update()
    {
        CheckWeaponKeys();
    }

    private void CheckWeaponKeys()
    {
        if (Input.GetKeyDown(KnifeKey))
        {
            currentActiveWeapon = Weapons.Knife;
            if(pistol.activeSelf) pistolAnimator.SetTrigger(IsPuttingAway);
            if(shotgun.activeSelf) shotgunAnimator.SetTrigger(IsPuttingAway);
            foreach (var image in weaponImages) image.color = Color.red;
            weaponImages[(int) Weapons.Knife].color = Color.green;
            knife.SetActive(true);
        }
        else if (Input.GetKeyDown(PistolKey))
        {
            currentActiveWeapon = Weapons.Pistol;
            if(knife.activeSelf) knifeAnimator.SetTrigger(IsPuttingAway);
            if(shotgun.activeSelf) shotgunAnimator.SetTrigger(IsPuttingAway);
            foreach (var image in weaponImages) image.color = Color.red;
            weaponImages[(int) Weapons.Pistol].color = Color.green;
            pistol.SetActive(true);
        }
        else if (Input.GetKeyDown(RifleKey))
        {
            foreach (var image in weaponImages) image.color = Color.red;
            weaponImages[(int) Weapons.Rifle].color = Color.green;
            currentActiveWeapon = Weapons.Rifle;
        }
        else if (Input.GetKeyDown(ShotgunKey))
        {
            currentActiveWeapon = Weapons.Shotgun;
            if(pistol.activeSelf) pistolAnimator.SetTrigger(IsPuttingAway);
            if(knife.activeSelf) knifeAnimator.SetTrigger(IsPuttingAway);
            foreach (var image in weaponImages) image.color = Color.red;
            weaponImages[(int) Weapons.Shotgun].color = Color.green;
            shotgun.SetActive(true);
        }
    }
    
    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        healthText.text = "Health: " + health + "/ " + maxHealth;

        if (health <= 0)
        {
            Die();
            health = 0;
        }
        else
            audioManager.Play("DamageTakenSound", playerAudioSource);
        
        healthText.text = "Health: " + health + "/ " + maxHealth;
        healthText.color = health < maxHealth / 2 ? Color.yellow : Color.green;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (health == 0) healthText.color = Color.red;
    }

    private void Die()
    {
        Debug.Log("Player Dead!");
        
        audioManager.Play("DeathSound", playerAudioSource);
        cameraDeathHolder.transform.parent = null;
        playerCamera.transform.parent = cameraDeathHolder.transform;

        foreach (var weapon in weaponArray)
            weapon.SetActive(false);
        
        cameraAnimator.SetTrigger(PlayerHasDied);
        Destroy(gameObject);
    }
}
