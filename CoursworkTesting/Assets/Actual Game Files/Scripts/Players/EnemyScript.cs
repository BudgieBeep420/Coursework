using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.Serialization;
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

    private float _timeToStartEngaging;
    private bool _hasSeenPlayer;
    private PistolScript _pistolScript;
    
    private void Start()
    {
        _pistolScript = enemyWeapon.GetComponent<PistolScript>();
    }

    private void Awake()
    {
        _timeToStartEngaging = Random.Range(0f, 1f);
    }

    private void Update()
    {
        CheckEnemyFieldOfView();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health < 1)
            Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyViewRadius);
    }

    private void CheckEnemyFieldOfView()
    {
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
