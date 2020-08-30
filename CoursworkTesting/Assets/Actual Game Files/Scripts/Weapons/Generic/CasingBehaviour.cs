using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using Actual_Game_Files.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CasingBehaviour : MonoBehaviour
{
    [Header("Game Settings")] 
    [SerializeField] private float casingLife;
    [SerializeField] private float casingEjectionSpeed;
    [Space] 
    
    [Header("Game Objects")] 
    [SerializeField] private AudioManager audioManager;
    [Space]    
    
    private float _casingLifeLength;
    private Transform _bulletSpawnTransform;
    private Rigidbody _casingRb;
    private AudioSource _thisAudioSource;

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _bulletSpawnTransform = GameObject.Find("CasingSpawn").transform;
        _casingRb = gameObject.GetComponent<Rigidbody>();
        _thisAudioSource = gameObject.GetComponent<AudioSource>();
        _casingRb.velocity = (_bulletSpawnTransform.right - _bulletSpawnTransform.forward) * casingEjectionSpeed;
        gameObject.transform.SetParent(GameObject.FindWithTag("CasingsBin").transform);
        StartCoroutine(CasingSound());
    }
    
    private void Update()
    {
        _casingLifeLength += Time.deltaTime;

        if (_casingLifeLength > casingLife)
            Destroy(gameObject);
    }

    private IEnumerator CasingSound()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));

        audioManager.Play(Random.Range(0, 2) == 0 ? "PistolCasingSound1" : "PistolCasingSound2", _thisAudioSource);
    }
}
