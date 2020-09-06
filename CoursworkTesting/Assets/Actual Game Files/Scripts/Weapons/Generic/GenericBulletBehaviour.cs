using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Lean.Transition;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class GenericBulletBehaviour : MonoBehaviour
{
    protected abstract Rigidbody ThisBulletRb { get; set; }
    protected abstract GameObject ThisBullet { get; set; }
    protected abstract GameObject BulletBin { get; set; } 
    protected abstract GameObject BloodSquirt { get; set; }
    protected abstract PlayerBehaviourScript PlayerBehaviourScript { get; set; }
    
    protected abstract float BulletDamage { get; set; }
    protected abstract float BulletPushBack { get; set; }

    protected abstract float BulletSpeed { get; set; }
    private const int EnvironmentLayer = 10;
    private int _enemyLayer;
    private Vector3 _pushBack;

    public float lifetimeOfBullet;

    private void Start()
    {
        _pushBack = transform.forward.normalized * BulletPushBack;
    }
    private void Update()
    {
        UpdateLifetimeCounter();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (lifetimeOfBullet < 0.005f) return;
        if (lifetimeOfBullet > 3) Destroy(gameObject);
        
        if (col.CompareTag("Enemies"))
        {
            Debug.Log("Enemy hit!");
            col.GetComponent<EnemyScript>().TakeDamage(GenerateRandomBulletDmg(BulletDamage));
            Instantiate(BloodSquirt, transform.position, Quaternion.Euler(new Vector3(-115, 0, 0)));
        }

        if (col.CompareTag("Player"))
        {
            PlayerBehaviourScript.TakeDamage(GenerateRandomBulletDmg(BulletDamage));
            Debug.Log("Player Hit!");
            col.GetComponent<CharacterController>().Move(_pushBack);
        }

        if (col.gameObject.layer == EnvironmentLayer)
        {
            Debug.Log("Environment Hit");
        }

        Destroy(gameObject);
    }
    
    private static float GenerateRandomBulletDmg(float baseDamage)
    {
        var lower = Convert.ToSingle(baseDamage - 0.1 * baseDamage);
        var upper = Convert.ToSingle(baseDamage + 0.1 * baseDamage);
        return Convert.ToSingle(Math.Round(Random.Range(lower, upper)));
    }
    
    protected void SetBulletSpeed()
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
