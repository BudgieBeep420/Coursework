using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

public class CasingBehaviour : MonoBehaviour
{
    [Header("Game Settings")] 
    [SerializeField] private float casingLife;
    [SerializeField] private float casingEjectionSpeed;
    
    
    private float casingLifeLength;
    private Transform bulletSpawnTransform;
    private Rigidbody casingRb;

    private void Awake()
    {
        bulletSpawnTransform = GameObject.Find("CasingSpawn").transform;
        casingRb = gameObject.GetComponent<Rigidbody>();
        casingRb.velocity = (bulletSpawnTransform.right + bulletSpawnTransform.forward) * casingEjectionSpeed;

    }
    
    private void Update()
    {
        casingLifeLength += Time.deltaTime;

        if (casingLifeLength > casingLife)
            Destroy(gameObject);
    }
}
