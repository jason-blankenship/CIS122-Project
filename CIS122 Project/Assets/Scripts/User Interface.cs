using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UserInterface : MonoBehaviour
{
    // Player Health variables
    public PlayerHealth playerHealth;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;

    //Reload Variables
    public Gun gun;
    public TextMeshProUGUI ammoText;


    // Update is called once per frame
    void Update()
    {
        //Player Health Hud Elements
        healthText.text = $"Health: {playerHealth.CurrHealth.ToString()} ";

        healthSlider.value = playerHealth.currHealth / playerHealth.maxHealth;

        //Gun Hud Elements
        ammoText.text = $"Ammo : {gun.currrentAmmoForHud.ToString()}";
    }
}
