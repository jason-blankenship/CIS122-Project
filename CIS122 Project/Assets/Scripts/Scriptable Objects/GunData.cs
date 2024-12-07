// written by jason blankenship

using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;


[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{


    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;          // in rounds per minute
    public float reloadTime;
    public int maxAmmoReserve;
    public int currentAmmoReserve;

    [HideInInspector]
    public bool reloading;

}
