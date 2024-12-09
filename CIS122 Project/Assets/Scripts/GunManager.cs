// written by jason blankenship

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject currentGun;
    public Transform gunHolder;

    public Gun currentGunScript;

    public void EquipNewGun(GameObject newGunPreFab)
    {
        
        

        if (currentGun != null)
        {
            

            if (currentGunScript != null)
            {
                PlayerShoot.shootInput -= currentGunScript.Shoot;
                PlayerShoot.reloadInput -= currentGunScript.StartReload;
            }

            Destroy(currentGun);
        }

        currentGun = Instantiate(newGunPreFab, gunHolder.position, gunHolder.rotation, gunHolder);
        
        currentGunScript = currentGun.GetComponent<Gun>();

        if (currentGunScript != null)
        {
            
            currentGunScript.gunData.currentAmmoReserve = currentGunScript.gunData.maxAmmoReserve;
            currentGunScript.gunData.currentAmmo = currentGunScript.gunData.magSize;

            
            PlayerShoot.shootInput += currentGunScript.Shoot;
            PlayerShoot.reloadInput += currentGunScript.StartReload;
        }
    }

    public void MaxAmmo()
    {
        if (currentGunScript != null)
        {
            currentGunScript.gunData.currentAmmoReserve = currentGunScript.gunData.maxAmmoReserve;
            currentGunScript.gunData.currentAmmo = currentGunScript.gunData.magSize;
        }
    
    }
}
