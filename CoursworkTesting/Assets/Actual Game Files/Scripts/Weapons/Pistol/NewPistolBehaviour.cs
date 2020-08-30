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
    [Space]
            
    [Header("Game Settings")]
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float pistolInaccuracyInDegrees;
    [SerializeField] private Magazine[] magazineArray;
    [SerializeField] private int weaponMagCapacity;
    [Space]

    [Header("SoundNames")]
    [SerializeField] private string pistolShotSoundName;
    [Space]
        
    private readonly bool canShoot = true;
    private readonly bool canReload = true;
    
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
        CanShoot = canShoot;
        CanReload = canReload;
        WhereCasingSpawns = whereCasingsSpawn;
        MagazineArray = magazineArray;
        AmmoText = ammoText;
        WeaponMagCapacity = weaponMagCapacity;
        MagazineText = magazineText;
    }

    private void Update()
    {
        ShootingWeaponCheck();
        CheckReload();
    }
}
