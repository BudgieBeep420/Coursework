using System;
using System.Collections;
using System.Resources;
using System.Security.Cryptography;
using Actual_Game_Files.Scripts;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float enemyViewRadius = 15f;
    [SerializeField] private float enemyRotationSpeed = 3f;
    [SerializeField] public float health;
    [Space]
    
    [Header("Objects")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject enemyWeapon;
    [SerializeField] private AudioSource thisAudioSource;
    [SerializeField] private GameObject deathAudioObject;

    private float _timeToStartEngaging;
    private bool _hasSeenPlayer;
    private PistolScript _pistolScript;
    private AudioManager _audioManager;
    private GameManagerScript _gameManagerScript;


    private void Start()
    {
        _gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        _pistolScript = enemyWeapon.GetComponent<PistolScript>();
        _audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        _timeToStartEngaging = Random.Range(0f, 2f);
    }

    private void Update()
    {
        if(playerTransform != null && CanSeePlayer(
            Vector3.Distance(playerTransform.position, transform.position)))
        {
            CheckEnemyFieldOfView();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health < 1)
            Die();
        else
            _audioManager.Play("DamageTakenSound", thisAudioSource);
    }

    private void Die()
    {
        deathAudioObject.transform.parent = null;
        _audioManager.Play("DeathSound", deathAudioObject.GetComponent<AudioSource>());
        _gameManagerScript.NumberOfKills++;
        Debug.Log("Number of kills: " + _gameManagerScript.NumberOfKills);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyViewRadius);
    }

    private bool CanSeePlayer(float distance)
    {
        var isBlocked = false;
        var ray = new Ray(transform.position, 
            (playerTransform.position - transform.position).normalized); ;
        
        if (Physics.Raycast(ray, out var hit, distance))
        {
            if (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 11) isBlocked = true;
        }
        return !isBlocked;
    }

    private void CheckEnemyFieldOfView()
    {
        if (playerTransform == null) return;
        
        var distance = Vector3.Distance(playerTransform.position, transform.position);
        
        if (distance > enemyViewRadius) return;

        if (!_hasSeenPlayer)
        {
            StartCoroutine(CountdownTillAttack());
            _hasSeenPlayer = true;
        }
        
        agent.SetDestination(playerTransform.position);

        if(distance <= agent.stoppingDistance)
            MakeEnemyFaceTarget();
    }
    
    private IEnumerator CountdownTillAttack()
    {
        yield return new WaitForSeconds(_timeToStartEngaging);
        ShootAtPlayer();
    }

    private void MakeEnemyFaceTarget()
    {
        var direction = (playerTransform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * enemyRotationSpeed);
    }

    private void ShootAtPlayer()
    {
        _pistolScript.Shoot();
    }
}
