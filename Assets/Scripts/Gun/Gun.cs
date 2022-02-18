using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform shootPoint;

    [Header("Config")]
    [SerializeField] public string gunName;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float shootRate;
    [SerializeField] public int maxAmmo;
    [HideInInspector] public int currentAmmo;

    //Shoot variables
    float timeBetweenShots => 1f / shootRate;
    float shootRateTimer = 0f;

    void Update()
    {
        shootRateTimer += Time.deltaTime;
    }

    public void Pickup()
    {
        currentAmmo = maxAmmo;
        transform.localPosition = Vector3.zero;
    }

    public void AttemptShoot()
    {
        Debug.Log("Attempting shoot");

        if (shootRateTimer < timeBetweenShots)
            return;

        if (currentAmmo <= 0)
            return;

        Shoot();
    }

    void Shoot()
    {
        Debug.Log("Shoot");

        shootRateTimer = 0f;
        currentAmmo--;

        Bullet shotBullet = Instantiate(bulletPrefab, shootPoint.transform.position, transform.rotation);
        shotBullet.Shoot(transform.forward);
    }
}
