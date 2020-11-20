using Actual_Game_Files.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShotgunBehaviourScript : GenericWeaponBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject shotBullet;
    [SerializeField] private GameObject shell;
    [SerializeField] private GameObject endOfBarrel;
    [SerializeField] private Animator shotgunAnimator;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject whereShellsSpawn;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text magazineText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource slideHandleAudioSource;
    [SerializeField] private GameObject[] animatedShells;
    [Space]
            
    [Header("Game Settings")]
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float shotgunInaccuracyInDegrees;
    [SerializeField] private Magazine[] magazineArray;
    [SerializeField] private int weaponMagCapacity;
    [SerializeField] private float shotgunReloadTime;
    [Space]

    [Header("SoundNames")]
    [SerializeField] private string shotgunShotSoundName;
    [SerializeField] private string shotgunReloadSoundName;
    
    protected override GameObject ShotBullet { get; set; }
    protected override GameObject BulletCasing { get; set; }
    protected override GameObject WhereCasingSpawns { get; set; }
    protected override GameObject EndOfBarrel { get; set; }
    protected override Animator WeaponAnimator { get; set; }
    protected override AudioManager AudioManager { get; set; }
    protected override Text AmmoText { get; set; }
    protected override Text MagazineText { get; set; }
    protected override float TimeBetweenShots { get; set; }
    protected override float WeaponInaccuracyInDegrees { get; set; }
    protected override Magazine[] MagazineArray { get; set; }
    protected override int WeaponMagCapacity { get; set; }
    protected override string WeaponShotSoundName { get; set; }
    protected override string WeaponReloadSoundName { get; set; }
    protected override bool IsWeaponFullyAutomatic { get; set; }
    protected override bool IsWeaponShotgun { get; set; }
    protected override float WeaponReloadTime { get; set; }
    protected override AudioSource ThisAudioSource { get; set; }

    private void Awake()
    {
        ShotBullet = shotBullet;
        BulletCasing = shell;
        WhereCasingSpawns = whereShellsSpawn;
        EndOfBarrel = endOfBarrel;
        WeaponAnimator = shotgunAnimator;
        AudioManager = audioManager;
        AmmoText = ammoText;
        MagazineText = magazineText;
        TimeBetweenShots = timeBetweenShots;
        WeaponInaccuracyInDegrees = shotgunInaccuracyInDegrees;
        MagazineArray = magazineArray;
        WeaponMagCapacity = weaponMagCapacity;
        WeaponReloadSoundName = shotgunReloadSoundName;
        WeaponShotSoundName = shotgunShotSoundName;
        MagazineArray = magazineArray;
        WeaponReloadTime = shotgunReloadTime;
        ThisAudioSource = audioSource;
        CanShoot = true;
        CanReload = true;
        IsWeaponFullyAutomatic = false;
        IsWeaponShotgun = true;
    }
    
    private void Update()
    {
        ShootingWeaponCheck();
        CheckReload();
    }

    private void PlayCycleSound()
    {
        audioManager.Play(Random.Range(0, 2) == 0 ? "Cycle1" : "Cycle2", slideHandleAudioSource);
    }

    private void DisableShells()
    {
        foreach (var shell in animatedShells) shell.SetActive(false);
    }

    private void EnableShells()
    {
        foreach (var shell in animatedShells) shell.SetActive(true);
    }

    private void LoadingShellSound()
    {
        audioManager.Play("ShotgunReloadSound", audioSource);
    }
}
