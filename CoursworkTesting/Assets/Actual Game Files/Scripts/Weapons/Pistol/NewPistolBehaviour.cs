using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Actual_Game_Files.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class NewPistolBehaviour : GenericWeaponBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject shotBullet;
    [SerializeField] private GameObject bulletCasing;
    [SerializeField] private GameObject endOfBarrel;
    [SerializeField] private Animator pistolAnimator;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject whereCasingsSpawn;
    [SerializeField] private GameObject casingsBin;
    [Space]
            
    [Header("Game Settings")]
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float pistolInaccuracyInDegrees = 10f;
    [Space]

    [Header("SoundNames")]
    [SerializeField] private string pistolShotSoundName;
    [Space]
        
    private bool canShoot = true;
    private static readonly int HasShot = Animator.StringToHash("HasShot");
    
    
    protected override GameObject ShotBullet { get; set; }
    protected override GameObject BulletCasing { get; set; }
    protected override GameObject WhereCasingSpawns { get; set; }
    protected override GameObject EndOfBarrel { get; set; }
    protected override Animator WeaponAnimator { get; set; }
    protected override AudioManager AudioManager { get; set; }
    protected override GameObject CasingsBin { get; set; }
    protected override float TimeBetweenShots { get; set; }
    protected override float WeaponInaccuracyInDegrees { get; set; }
    protected override string WeaponShotSoundName { get; set; }
    protected override bool CanShoot { get; set; }

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
        WhereCasingSpawns = whereCasingsSpawn;
        CasingsBin = casingsBin;
    }

    private void Update()
    {
        ShootingWeaponCheck();
    }
}
