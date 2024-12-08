using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{

    private float health = 100f;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            WaveSpawner waveSpawner = FindObjectOfType<WaveSpawner>();
            if (waveSpawner != null )
            {
                waveSpawner.ZombieDestroyed();
            }
            Destroy(gameObject);
        }
    }
}
