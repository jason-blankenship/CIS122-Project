using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{

    //To add Money


    private float health = 100f;
    public void TakeDamage(float damage)
    {
        ShopManager shopmanager = FindAnyObjectByType<ShopManager>();
        shopmanager.addMoney();

        health -= damage;
        if (health <= 0)
        {
            // Nathan Stoffel
            WaveSpawner waveSpawner = FindObjectOfType<WaveSpawner>();
            if (waveSpawner != null )
            {
                waveSpawner.ZombieDestroyed();
            }
            Destroy(gameObject);
        }
    } 

}
