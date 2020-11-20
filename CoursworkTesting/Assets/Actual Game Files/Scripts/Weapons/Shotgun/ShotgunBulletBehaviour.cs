using UnityEngine;

public class ShotgunBulletBehaviour : GenericBulletBehaviour
{
    [Header("Objects")]
    [SerializeField] private Rigidbody thisBulletRb;
    [SerializeField] private GameObject thisBullet;
    [SerializeField] private GameObject bloodSquirt;
    [Space]
    
    [Header("Settings")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float bulletPushBack;

    private const string PlayerTag = "Player";

    protected override Rigidbody ThisBulletRb { get; set; }
    protected override GameObject ThisBullet { get; set; }
    protected override GameObject BulletBin { get; set; }
    protected override GameObject BloodSquirt { get; set; }
    protected override PlayerBehaviourScript PlayerBehaviourScript { get; set; }
    protected override float BulletDamage { get; set; }
    protected override float BulletPushBack { get; set; }
    protected override GameManagerScript GameManager { get; set; }
    protected override float BulletSpeed { get; set; }

    private void Awake()
    {
        ThisBulletRb = thisBulletRb;
        ThisBullet = thisBullet;
        BulletBin = GameObject.FindWithTag("BulletBin");
        BulletSpeed = bulletSpeed;
        BulletDamage = bulletDamage;
        BulletPushBack = bulletPushBack;
        BloodSquirt = bloodSquirt;
        GameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        
        if(GameObject.FindWithTag(PlayerTag) != null)
            PlayerBehaviourScript = GameObject.FindWithTag(PlayerTag).GetComponent<PlayerBehaviourScript>();

        SetBulletSpeed();
        SetBulletParent();
    }
}
