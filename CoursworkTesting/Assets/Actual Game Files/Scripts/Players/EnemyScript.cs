using System.Collections;
using System.Runtime.Remoting.Messaging;
using Actual_Game_Files.Scripts;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float enemyViewRadius = 15f;
    [SerializeField] private float enemyRotationSpeed = 3f;
    [SerializeField] public float health;
    [SerializeField] private bool isPatrol;
    [SerializeField] private float enemyViewAngle = 40f;
    [Space]
    
    [Header("Objects")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject enemyWeapon;
    [SerializeField] private AudioSource thisAudioSource;
    [SerializeField] private GameObject deathAudioObject;
    [SerializeField] private Transform endOfPistolTransform;
    [SerializeField] private GameObject waypointBin;

    private float _timeToStartEngaging;
    private bool _hasSeenPlayer;
    private PistolScript _pistolScript;
    private AudioManager _audioManager;
    private GameManagerScript _gameManagerScript;

    private float enemyHealthMult;

    private bool _isBlocked;
    private bool _canDie = true;


    /* This intialises all of the non-prefab GameObject which are referenced */
    private void Start()
    {
        _gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        _pistolScript = enemyWeapon.GetComponent<PistolScript>();
        _audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        _timeToStartEngaging = Random.Range(0.5f, 2f);
        
        /* Grabs the value of the enemy's health */
        health *= _gameManagerScript.currentDifficulty.enemyHealthMult;
        Debug.Log("New enemy health: " + health);
    }

    private void Update()
    {
        /* If the enemy can see the player ... */
        if (playerTransform != null && CanSeePlayer(Vector3.Distance(playerTransform.position, transform.position)))
        {
            _pistolScript.canSeePlayer = true;
            CheckEnemyFieldOfView();
        }
        else
        {
            _pistolScript.canSeePlayer = false;
            if (isPatrol && !agent.hasPath) StartRandomPatrol();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        _hasSeenPlayer = true;
        StartCoroutine(CountdownTillAttack());

        /* This validation makes sure that the enemy dies only once, when their health goes below 1 */
        /* This is called when a bullet enters the collider of an enemy */
        if (health < 1 && _canDie)
        {
            _canDie = !_canDie;
            Die();
        }
        else
            _audioManager.Play("DamageTakenSound", thisAudioSource);
    }

    private void Die()
    {
        /* This manages what happens after an enemy dies, it spawns an audio making object, as well as plays
            the death sound, and increments the number of kills the player has gotten, then deletes the enemy object*/
        
        deathAudioObject.transform.parent = null;
        _audioManager.Play("DeathSound", deathAudioObject.GetComponent<AudioSource>());
        _gameManagerScript.NumberOfKills++;
        
        Debug.Log("Number of kills: " + _gameManagerScript.NumberOfKills);
        Destroy(gameObject);
    }

    
    /* This allows for debugging */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyViewRadius);
    }

    private bool CanSeePlayer(float distance)
    {
        _isBlocked = false;
        var ray = new Ray(transform.position, 
            (playerTransform.position - transform.position).normalized); ;
        
        if (Physics.Raycast(ray, out var hit, distance))
        {
            if (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 11) _isBlocked = true;
        }
        
        return !_isBlocked;
    }

    private void CheckEnemyFieldOfView()
    {
        if (playerTransform == null) return;

        var playerPosition = playerTransform.position;
        var thisEnemyPosition = transform.position;
        var distance = Vector3.Distance(playerPosition, thisEnemyPosition);
        var angle = Vector3.Angle(transform.forward, playerPosition - thisEnemyPosition);
        
        if (distance > enemyViewRadius) return;

        if (angle > enemyViewAngle && !_hasSeenPlayer) return;
        
        if (!_hasSeenPlayer)
        {
            StartCoroutine(CountdownTillAttack());
            _hasSeenPlayer = true;
        }
        
        agent.SetDestination(playerTransform.position);

        if (!(distance <= agent.stoppingDistance)) return;
        MakeEnemyFaceTarget();
        MakeGunBarrelPointAtTarget();
    }
    
    private IEnumerator CountdownTillAttack()
    {
        yield return new WaitForSeconds(_timeToStartEngaging * _gameManagerScript.currentDifficulty.enemyReactionMult);
        ShootAtPlayer();
    }

    private void MakeEnemyFaceTarget()
    {
        var direction = (playerTransform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * enemyRotationSpeed);
    }

    private void MakeGunBarrelPointAtTarget()
    {
        endOfPistolTransform.LookAt(playerTransform);
        endOfPistolTransform.Rotate(Vector3.right, 90f);
    }

    private void ShootAtPlayer()
    {
        _pistolScript.Shoot();
    }
    
    private Vector3 RandomNavmeshLocation()
    {
        var x = Random.Range(1, 10);
        return waypointBin.transform.GetChild(x).position;
    }

    private void StartRandomPatrol()
    {
        agent.SetDestination(RandomNavmeshLocation());
        StartCoroutine(WaitForPatrol(10f));
    }

    private IEnumerator WaitForPatrol(float time)
    {
        yield return new WaitForSeconds(time);
        if(!_hasSeenPlayer) StartRandomPatrol();
    }
}
