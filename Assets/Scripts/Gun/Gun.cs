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
	[SerializeField] float reloadTime;

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
        if (shootRateTimer < timeBetweenShots)
            return;

        if (currentAmmo <= 0)
            return;

        Shoot();
    }

	Coroutine reloadCoroutine;

    void Shoot()
    {
        shootRateTimer = 0f;
        currentAmmo--;

        Bullet shotBullet = Instantiate(bulletPrefab, shootPoint.transform.position, transform.rotation);
        shotBullet.Shoot(transform.forward);

		if(currentAmmo <= 0 && reloadCoroutine == null) {
			reloadCoroutine = StartCoroutine(ReloadCoroutine());
		}

    }

	IEnumerator ReloadCoroutine() {
		yield return new WaitForSeconds(reloadTime);

		currentAmmo = maxAmmo;
		reloadCoroutine = null;
	}


}
