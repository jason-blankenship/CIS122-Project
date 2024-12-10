// written by jason blankenship

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
       {
            ShopManager.instance.ToggleShop();
       }

        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShopManager.instance.ToggleShop();
        }
    }
}
