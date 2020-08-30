using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class GenericBulletBehaviour : MonoBehaviour
{
    protected abstract Rigidbody ThisBulletRb { get; set; }
    protected abstract GameObject ThisBullet { get; set; }
    protected abstract GameObject BulletBin { get; set; }
    
    protected abstract float BulletDamage { get; set; }

    protected abstract float BulletSpeed { get; set; }
    private int _environmentLayer;
    private int _enemyLayer;

    public float lifetimeOfBullet;

    private void Awake()
    {
        _environmentLayer = LayerMask.NameToLayer("Environment");
    }

    private void Update()
    {
        UpdateLifetimeCounter();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (lifetimeOfBullet < 0.005f) return;
        
        if (col.CompareTag("Enemies"))
        {
            Debug.Log("Enemy hit!");
            col.GetComponent<EnemyScript>().TakeDamage(BulletDamage);
        }

        if (col.CompareTag("Player"))
        {
            Debug.Log("Player Hit!");
        }
    }

    /*private static void UpdateEnemyHealth(GameObject enemyObject)
    {
        Debug.Log(enemyObject.name);
        enemyObject.GetComponent<EnemyScript>().enemyLifeCount--;
        if (enemyObject.GetComponent<EnemyScript>().enemyLifeCount != 0) return;
        Destroy(enemyObject);
    }*/
    protected void SetBulletSpeedAndTrajectory()
    {
        ThisBulletRb.velocity = ThisBullet.transform.up * (BulletSpeed * Time.deltaTime);
    }

    protected void SetBulletParent()
    {
        ThisBullet.transform.SetParent(BulletBin.transform);
    }

    private void UpdateLifetimeCounter()
    {
        lifetimeOfBullet += Time.deltaTime;
    }
}
