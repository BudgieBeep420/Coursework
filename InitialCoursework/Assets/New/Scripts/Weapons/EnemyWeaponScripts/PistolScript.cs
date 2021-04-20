using System.Collections;
using Actual_Game_Files.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class PistolScript : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject shotBullet;
    [SerializeField] private GameObject bulletCasing;
    [SerializeField] private GameObject whereCasingSpawns;
    [SerializeField] private GameObject endOfBarrel;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private AudioManager audioManager;
    [Space]
    
    [Header("Game Settings")]
    [SerializeField] private int numberOfBullets;
    [SerializeField] private float weaponInaccuracyInDegrees;
    [SerializeField] private float weaponShootingCooldown;
    [Space]
    
    [Header("SoundNames")]
    [SerializeField] private string pistolShotSoundName;
    [Space]
    
    private static readonly int HasShot = Animator.StringToHash("HasShot");
    private const float CasingRotation = 20f;
    private static Transform _playerTransform;
    private AudioSource _thisAudioSource;

    public bool canSeePlayer;

    private void Awake()
    {
        _thisAudioSource = gameObject.GetComponent<AudioSource>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }
    
    public void Shoot()
    {
        /* This function enters a recursive loop, which shoots all the pistols bullets */
        
        StartCoroutine(ShootingCooldown(weaponShootingCooldown));
        if (_playerTransform == null || !canSeePlayer) return;
        
        if (numberOfBullets == 0)
        {
            audioManager.Play("OutOfAmmoClick", _thisAudioSource);
            return;
        }
        
        /* This does the actual mechanics of the gun shot */
        
        numberOfBullets--;
        weaponAnimator.SetBool(HasShot, true);
        audioManager.Play(pistolShotSoundName, _thisAudioSource);

        Instantiate(shotBullet, endOfBarrel.transform.position,
            GenerateRandomBulletRotation(endOfBarrel.transform.rotation, weaponInaccuracyInDegrees));

        SpawnBulletCasing();
    }

    //This was changed to stop when the player turns a corner after being spotted
    private IEnumerator ShootingCooldown(float time)
    {
        /* Takes a temp variable, whether the enemy saw the player when it entered this method,
            then after waiting a given time, it checks if it can still see the player. If not, it 
            aborts shooting, and recursively carrys on waiting until it sees the player again 
            (i.e. it is on alert mode */
        var x = canSeePlayer;
        yield return new WaitForSeconds(Random.Range(0.3f * time, time));
        if(x == canSeePlayer) Shoot();
        else StartCoroutine(ShootingCooldown(weaponShootingCooldown));
    }
    
    private static Quaternion GenerateRandomBulletRotation(Quaternion initialRotation, float inaccuracy)
    {
        var randomX = Random.Range(-inaccuracy, inaccuracy) + 0.75;
        var randomY = Random.Range(-inaccuracy, inaccuracy) - 0.75;
        var randomZ = Random.Range(-inaccuracy, inaccuracy);

        return Quaternion.Euler((float)randomX, (float)randomY, (float)randomZ) * initialRotation;
    }
        
    private static Quaternion GenerateRandomCasingRotation(Quaternion initialRotation, float inaccuracy)
    {
        var randomX = Random.Range(-inaccuracy, inaccuracy);
        var randomY = Random.Range(-inaccuracy, inaccuracy);
        var randomZ = Random.Range(-inaccuracy, inaccuracy);

        return Quaternion.Euler(randomX, randomY, randomZ) * initialRotation;
    }
    
    private void SpawnBulletCasing()
    {
        var casing = Instantiate(bulletCasing);
        casing.transform.position = whereCasingSpawns.transform.position;
        casing.transform.rotation =
            GenerateRandomCasingRotation(whereCasingSpawns.transform.rotation, CasingRotation);
    }
    
    public void PlayTakingOutSound()
    {}
}
