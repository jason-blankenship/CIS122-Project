// written by jason blankenship

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunData gunData;
    [SerializeField] private Transform cam;
    
    //For Hud (Cole)
    public int currrentAmmoForHud;

    

    float timeSinceLastShot;
    int bulletsFired;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    public void StartReload()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(Reload());
        }

    }

    private IEnumerator Reload()
    {
        if (gunData.currentAmmoReserve > 0)
        {
            gunData.reloading = true;
            bulletsFired = gunData.magSize - gunData.currentAmmo;

            yield return new WaitForSeconds(gunData.reloadTime);

            if (gunData.currentAmmoReserve >= bulletsFired)
            {

                gunData.currentAmmo += bulletsFired;
                currrentAmmoForHud = gunData.currentAmmo;
                gunData.currentAmmoReserve -= bulletsFired;
            }
            else if (gunData.currentAmmoReserve < bulletsFired)
            {
                gunData.currentAmmo += gunData.currentAmmoReserve;
                currrentAmmoForHud = gunData.currentAmmo;
                gunData.currentAmmoReserve = 0;
            }

            gunData.reloading = false;
        }
    }


    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60);

    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.damage);

                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
                currrentAmmoForHud--;
            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(cam.position, cam.forward);
    }

    private void OnGunShot()
    {
        
    }
}
