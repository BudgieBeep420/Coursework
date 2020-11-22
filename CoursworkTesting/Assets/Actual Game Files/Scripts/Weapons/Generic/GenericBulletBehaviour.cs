using System;
using UnityEngine;
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
    protected abstract GameManagerScript GameManager { get; set; }
    public abstract bool IsBulletOfPlayer { get; set; }

    protected abstract float BulletSpeed { get; set; }
    private const int EnvironmentLayer = 10;
    private int _enemyLayer;
    private Vector3 _pushBack;
    private bool _isBloodEnabled;
    private const float PushBackFactor = 10;
    
    // This is gonna be changed depending on the difficulty
    private float _enemyDamageModifier = 0.2f;
    
    

    public float lifetimeOfBullet;

    private void Start()
    {
        _pushBack = transform.up.normalized * BulletPushBack * PushBackFactor;
        _isBloodEnabled = Convert.ToBoolean(GameManager.gameSettingsProfile.blood);
    }
    private void Update()
    {
        UpdateLifetimeCounter();
    }

    public void OnTriggerEnter(Collider col)
    {
        /*if (lifetimeOfBullet < 0.000001f) return;*/
        if (lifetimeOfBullet > 3) Destroy(gameObject);

        if (col.CompareTag("Enemies"))
        {
            col.GetComponent<EnemyScript>().TakeDamage(GenerateRandomBulletDmg(BulletDamage, false));
            
            if(_isBloodEnabled)
                Instantiate(BloodSquirt, transform.position, Quaternion.Euler(new Vector3(-115, 0, 0)));
        }

        if (col.CompareTag("Player"))
        {
            if (IsBulletOfPlayer) return;
            PlayerBehaviourScript.TakeDamage(GenerateRandomBulletDmg(BulletDamage, true));
            /*col.GetComponent<CharacterController>().Move(_pushBack);*/
        }

        if(col.attachedRigidbody) col.attachedRigidbody.AddForce(_pushBack);

        if (col.gameObject.layer == EnvironmentLayer)
        {
            // nice shot idiot 
        }

        Destroy(gameObject);
    }
    
    private float GenerateRandomBulletDmg(float baseDamage, bool isPlayer)
    {
        var lower = Convert.ToSingle(baseDamage - 0.1 * baseDamage);
        var upper = Convert.ToSingle(baseDamage + 0.1 * baseDamage);
        return Convert.ToSingle(!isPlayer ? Math.Round(Random.Range(lower, upper)) : Math.Round(_enemyDamageModifier * Random.Range(lower, upper)));
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
