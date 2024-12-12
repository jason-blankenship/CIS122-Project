using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// COLE GIBEAU

public class UserInterface : MonoBehaviour
{
    // Player Health variables
    public PlayerHealth playerHealth;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    public TextMeshProUGUI healsText;

    //Reload Variables
    public GunManager gunManager;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI ammoReserve;

    //Current Wave variables
    public TextMeshProUGUI waveText;

    //Money Variables
    public TextMeshProUGUI moneyText;


    // Update is called once per frame
    void Update()
    {
        
        //Player Health Hud Elements
        healthText.text = $"Health : {playerHealth.CurrHealth.ToString()} ";

        healthSlider.value = playerHealth.currHealth / playerHealth.maxHealth;

        healsText.text = "Heals : " + playerHealth.numberOfHeals;

        //Gun Hud Elements
        ammoText.text = $"Ammo : {gunManager.currentGunScript.gunData.currentAmmo.ToString()}";
        ammoReserve.text =gunManager.currentGunScript.gunData.currentAmmoReserve.ToString();


    }

    public void UpdateWaveDisplay(int currentWave)
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }
    }

    public void UpdateMoney(int money)
    {
        if (moneyText != null)
        {
            moneyText.text = "$" + money;
        }
    }
}
