using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class GenericBulletBehaviour : MonoBehaviour
{
    protected abstract Rigidbody ThisBulletRb { get; set; }
    protected abstract GameObject ThisBullet { get; set; }
    protected abstract GameObject BulletBin { get; set; }

    protected abstract float BulletSpeed { get; set; }
    private int environmentLayer;

    private void Awake()
    {
        environmentLayer = LayerMask.NameToLayer("Environment");
    }

    protected void OnTriggerEnter(Collider col)
    {
        Debug.Log("Hit");
        // Debug.Log(collider.name);
        
        if (col.gameObject.layer == environmentLayer)
        {
            Debug.Log("Environment Hit");
            Debug.Log(col.name);
            Destroy(ThisBullet);
        }
        
    }

    protected void SetBulletSpeedAndTrajectory()
    {
        ThisBulletRb.velocity = ThisBullet.transform.up * (BulletSpeed * Time.deltaTime);
    }

    protected void SetBulletParent()
    {
        ThisBullet.transform.SetParent(BulletBin.transform);
    }
}
