using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Written By Cole Gibeau 
public class PlayerHealth : MonoBehaviour, IDamageable
{

    public float maxHealth = 100f;
    public float currHealth;


    public float CurrHealth
    {
        get { return this.currHealth; }
    }

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        Debug.Log($"Health damaged {damage} amount");
    }
}
