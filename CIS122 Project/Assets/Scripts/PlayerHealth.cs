using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Written By Cole Gibeau 
public class PlayerHealth : MonoBehaviour, IDamageable
{

    public float maxHealth = 100f;
    public float currHealth;

    public int numberOfHeals;
    public float healthGainedFromHeal = 30f;
    
    //For Axe Swing Animation 
    public AxeSwingController axeSwingController;

    public float CurrHealth
    {
        get { return this.currHealth; }
    }

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        

        if(axeSwingController == null)
        {
            axeSwingController = FindObjectOfType<AxeSwingController>();
        }
    }

    public void TakeDamage(float damage)
    {
        if (currHealth >= 0) { currHealth -= damage; }
        Debug.Log($"Health damaged {damage} amount");

        if (axeSwingController != null)
        {
            axeSwingController.SwingAxe();
        }
    }

    // written by jason (for healing item)

    public void HealPlayer()
    {
        if (numberOfHeals > 0)
        {
            numberOfHeals--;
            if (currHealth + healthGainedFromHeal <= maxHealth)
            {
                currHealth += healthGainedFromHeal;
            }
            else
            {
                currHealth = maxHealth;
            }
        }
    }

    
}
