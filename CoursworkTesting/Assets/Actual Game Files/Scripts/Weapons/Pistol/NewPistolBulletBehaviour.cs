using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NewPistolBulletBehaviour : GenericBulletBehaviour
{
    [Header("Objects")]
    [SerializeField] private Rigidbody thisBulletRb;
    [SerializeField] private GameObject thisBullet;
    [Space]
    
    [Header("Settings")]
    [SerializeField] private float bulletSpeed;

    protected override Rigidbody ThisBulletRb { get; set; }
    protected override GameObject ThisBullet { get; set; }
    protected override GameObject BulletBin { get; set; }
    protected override float BulletSpeed { get; set; }

    private void Awake()
    {
        ThisBulletRb = thisBulletRb;
        ThisBullet = thisBullet;
        BulletBin = GameObject.FindWithTag("BulletBin");
        BulletSpeed = bulletSpeed;
        
        SetBulletSpeedAndTrajectory();
        SetBulletParent();
    }
}
