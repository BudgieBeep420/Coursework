using System.Collections;
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

    private bool _isBlocked;
    private bool _canDie = true;


    private void Start()
    {
        _gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        _pistolScript = enemyWeapon.GetComponent<PistolScript>();
        _audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        _timeToStartEngaging = Random.Range(0.5f, 3f);
    }

    private void Update()
    {
        if (playerTransform != null && CanSeePlayer(Vector3.Distance(playerTransform.position, transform.position)))
        {
            CheckEnemyFieldOfView();
        }
    }

    public void TakeDamage(float damage)
    {
        /* This is called every time a bullet enters the enemy. It decreases the health float
            in the enemy it hits. */
        health -= damage;

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
        /* Does a death animation, as well as plays a death sound when called */
        
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
        /* Sends out a ray between the enemy and the player */
        
        _isBlocked = false;
        var ray = new Ray(transform.position, 
            (playerTransform.position - transform.position).normalized); ;
        
        /* Checks any objects the ray hits, if its a player, it is fine, if it is a piece of the environment
            it will cause it to return false*/
        
        if (Physics.Raycast(ray, out var hit, distance))
        {
            if (hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 11) _isBlocked = true;
        }
        
        return !_isBlocked;
    }
    
    /* This is called every frame. It makes sure the player exists, then if the player
        is inside the Field of View, it will attack the player*/
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

        if (!(distance <= agent.stoppingDistance)) return;
        MakeEnemyFaceTarget();
        MakeGunBarrelPointAtTarget();
    }
    
    /* This is called to make the enemies' logic wait a few seconds until engaging */
    private IEnumerator CountdownTillAttack()
    {
        yield return new WaitForSeconds(_timeToStartEngaging);
        ShootAtPlayer();
    }

    private void MakeEnemyFaceTarget()
    {
        /* This turns the CharacterController around to look at the player, within certain bounds */
        var direction = (playerTransform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * enemyRotationSpeed);
    }

    private void MakeGunBarrelPointAtTarget()
    {
        /* This deals with the fact that the gun isnt looking at the player when the enemy looks at the
            player*/
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
