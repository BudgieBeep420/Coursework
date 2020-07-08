using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandgunBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject shotBullet;
    [SerializeField] private GameObject bulletCasing;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject endOfBarrel;
    [SerializeField] private Animator pistolAnimator;


    [SerializeField] private float _timeBetweenShots;

    private bool _canShoot = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnShootingPistol();
    }

    private void OnShootingPistol()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canShoot)
        {
            pistolAnimator.SetBool("HasShot", true);
            var bullet = Instantiate(shotBullet, endOfBarrel.transform.position, endOfBarrel.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * bulletSpeed * Time.deltaTime;
            StartCoroutine(WaitForTime(_timeBetweenShots));
        }
    }

    IEnumerator WaitForTime(float time)
    {
        _canShoot = false;

        yield return new WaitForSeconds(time);

        _canShoot = true;
    }

}
