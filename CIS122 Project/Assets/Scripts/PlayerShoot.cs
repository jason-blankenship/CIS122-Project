using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
   
    public static Action shootInput;
    public static Action reloadInput;

    //For Sounds (Cole)
    public AudioSource audioSourcePistol;
    public AudioClip pistolShot;
    public AudioSource audioSoruceReload;
    public AudioClip reload;


    [SerializeField] private KeyCode reloadKey;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            shootInput?.Invoke();

            //For Sound (COLE)
            audioSourcePistol.PlayOneShot(pistolShot);

        }

        if (Input.GetKeyDown(reloadKey))
        {
            reloadInput?.Invoke();

            //For Sound (Cole)
            audioSoruceReload.PlayOneShot(reload);
        }
    }
}
