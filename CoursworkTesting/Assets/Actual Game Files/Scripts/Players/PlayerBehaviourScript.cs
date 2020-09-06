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
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject shotgun;
    [Space]
    [SerializeField] private Animator knifeAnimator;
    [SerializeField] private Animator pistolAnimator;
    [SerializeField] private Animator rifleAnimator;
    [SerializeField] private Animator shotgunAnimator;
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
            knife.SetActive(true);
        }
        if (Input.GetKeyDown(PistolKey))
        {
            currentActiveWeapon = Weapons.Pistol;
            if(knife.activeSelf) knifeAnimator.SetTrigger(IsPuttingAway);
            if(shotgun.activeSelf) shotgunAnimator.SetTrigger(IsPuttingAway);
            pistol.SetActive(true);
        }
        if (Input.GetKeyDown(RifleKey))
        {
            currentActiveWeapon = Weapons.Rifle;
        }

        if (Input.GetKeyDown(ShotgunKey))
        {
            currentActiveWeapon = Weapons.Shotgun;
            if(pistol.activeSelf) pistolAnimator.SetTrigger(IsPuttingAway);
            if(knife.activeSelf) knifeAnimator.SetTrigger(IsPuttingAway);
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
    }

    private void Die()
    {
        Debug.Log("Player Dead!");
        
        audioManager.Play("DeathSound", playerAudioSource);
        cameraDeathHolder.transform.parent = null;
        playerCamera.transform.parent = cameraDeathHolder.transform;

        foreach (var weapon in weaponArray)
        {
            weapon.SetActive(false);
        }
        
        cameraAnimator.SetTrigger(PlayerHasDied);
        Destroy(gameObject);
    }
}
