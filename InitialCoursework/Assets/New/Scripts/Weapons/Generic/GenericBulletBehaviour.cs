using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class GenericBulletBehaviour : MonoBehaviour
{
    /* These are all defined by the deriving claseses */
    protected abstract Rigidbody ThisBulletRb { get; set; }
    protected abstract GameObject ThisBullet { get; set; }
    protected abstract GameObject BulletBin { get; set; } 
    protected abstract GameObject BloodSquirt { get; set; }
    protected abstract PlayerBehaviourScript PlayerBehaviourScript { get; set; }
    protected abstract float BulletDamage { get; set; }
    protected abstract float BulletPushBack { get; set; }
    protected abstract GameManagerScript GameManager { get; set; }
    public abstract bool IsBulletOfPlayer { get; set; }

    protected abstract float BulletSpeed { get; set; }
    
    /* These are variables used in this script */
    private const int EnvironmentLayer = 10;
    private int _enemyLayer;
    private Vector3 _pushBack;
    private bool _isBloodEnabled;
    private const float PushBackFactor = 10;
    
    // This is changed depending on the difficulty
    private float _enemyDamageModifier = 0.2f;

    public float lifetimeOfBullet;

    /* This initalises values for the push back effect of the bullet, and checks
        if blood is enabled by the player*/
    private void Start()
    {
        _pushBack = transform.up.normalized * BulletPushBack * PushBackFactor;
        _isBloodEnabled = Convert.ToBoolean(GameManager.gameSettingsProfile.blood);
    }
    
    private void Update()
    {
        UpdateLifetimeCounter();
    }

    /* This function is called when the collider of the bullet enters the collider of another object */
    public void OnTriggerEnter(Collider col)
    {
        /* If the bullet doesn't hit anything, then this is ran to clean up */
        if (lifetimeOfBullet > 3) Destroy(gameObject);

        /* If the bullet hits an enemy, it generates blood (or not), and does damage to them */
        if (col.CompareTag("Enemies"))
        {
            col.GetComponent<EnemyScript>().TakeDamage(GenerateRandomBulletDmg(BulletDamage, false));
            
            if(_isBloodEnabled)
                Instantiate(BloodSquirt, transform.position, Quaternion.Euler(new Vector3(-115, 0, 0)));
        }

        /* If it is a player, it checks if the bullet is from the player. If it is, it ignores, if not#
            the player will take damage */
        if (col.CompareTag("Player"))
        {
            if (IsBulletOfPlayer) return;
            PlayerBehaviourScript.TakeDamage(GenerateRandomBulletDmg(BulletDamage, true));
            /*col.GetComponent<CharacterController>().Move(_pushBack);*/
        }

        /* If the collidee has a RigidBody, then it is pushed backwards */
        if(col.attachedRigidbody) col.attachedRigidbody.AddForce(_pushBack);

        /* if it hits the environment, nothing happens */
        if (col.gameObject.layer == EnvironmentLayer)
        {
            // nice shot idiot 
        }

        Destroy(gameObject);
    }
    
    /* This varies the damage of the bullet by a bit */
    private float GenerateRandomBulletDmg(float baseDamage, bool isPlayer)
    {
        var lower = Convert.ToSingle(baseDamage - 0.1 * baseDamage);
        var upper = Convert.ToSingle(baseDamage + 0.1 * baseDamage);
        return Convert.ToSingle(!isPlayer ? Math.Round(Random.Range(lower, upper)) : Math.Round(_enemyDamageModifier * Random.Range(lower, upper)));
    }
    
    /* This is done to make sure the bullet actually moves */
    protected void SetBulletSpeed()
    {
        ThisBulletRb.velocity = ThisBullet.transform.up * (BulletSpeed * Time.deltaTime);
    }

    /* This cleans up the hierarchy */
    protected void SetBulletParent()
    {
        ThisBullet.transform.SetParent(BulletBin.transform);
    }

    private void UpdateLifetimeCounter()
    {
        lifetimeOfBullet += Time.deltaTime;
    }
}
