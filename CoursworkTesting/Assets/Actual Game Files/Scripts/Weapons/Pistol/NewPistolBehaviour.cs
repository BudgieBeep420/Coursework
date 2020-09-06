using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Actual_Game_Files.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NewPistolBehaviour : GenericWeaponBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject shotBullet;
    [SerializeField] private GameObject bulletCasing;
    [SerializeField] private GameObject endOfBarrel;
    [SerializeField] private Animator pistolAnimator;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject whereCasingsSpawn;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text magazineText;
    [SerializeField] private AudioSource audioSource;
    [Space]
            
    [Header("Game Settings")]
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float pistolInaccuracyInDegrees;
    [SerializeField] private Magazine[] magazineArray;
    [SerializeField] private int weaponMagCapacity;
    [SerializeField] private string weaponReloadSoundName;
    [SerializeField] private float weaponReloadTime = 2f;
    [Space]

    [Header("SoundNames")]
    [SerializeField] private string pistolShotSoundName;

    protected override GameObject ShotBullet { get; set; }
    protected override GameObject BulletCasing { get; set; }
    protected override GameObject WhereCasingSpawns { get; set; }
    protected override GameObject EndOfBarrel { get; set; }
    protected override Animator WeaponAnimator { get; set; }
    protected override AudioManager AudioManager { get; set; }
    protected override Text AmmoText { get; set; }
    protected override Text MagazineText { get; set; }
    protected override float TimeBetweenShots { get; set; }
    protected override float WeaponInaccuracyInDegrees { get; set; }
    protected override Magazine[] MagazineArray { get; set; }
    protected override int WeaponMagCapacity { get; set; }
    protected override string WeaponShotSoundName { get; set; }
    protected override string WeaponReloadSoundName { get; set; }
    protected override bool IsWeaponFullyAutomatic { get; set; }
    protected override bool IsWeaponShotgun { get; set; }
    protected override float WeaponReloadTime { get; set; }
    protected override AudioSource ThisAudioSource { get; set; }
    protected override bool CanShoot { get; set; }
    protected override bool CanReload { get; set; }


    private void Awake()
    {
        ShotBullet = shotBullet;
        BulletCasing = bulletCasing;
        EndOfBarrel = endOfBarrel;
        WeaponAnimator = pistolAnimator;
        AudioManager = audioManager;
        TimeBetweenShots = timeBetweenShots;
        WeaponInaccuracyInDegrees = pistolInaccuracyInDegrees;
        WeaponShotSoundName = pistolShotSoundName;
        CanShoot = true;
        CanReload = true;
        WhereCasingSpawns = whereCasingsSpawn;
        MagazineArray = magazineArray;
        AmmoText = ammoText;
        WeaponMagCapacity = weaponMagCapacity;
        MagazineText = magazineText;
        IsWeaponFullyAutomatic = false;
        IsWeaponShotgun = false;
        WeaponReloadSoundName = weaponReloadSoundName;
        WeaponReloadTime = weaponReloadTime;
        ThisAudioSource = audioSource;
    }

    private void Update()
    {
        ShootingWeaponCheck();
        CheckReload();
    }
}
