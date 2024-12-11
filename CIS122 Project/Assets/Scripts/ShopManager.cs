// written by jason blankenship

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public int money = 300;
    public Product[] products;

    // references
    public Text moneyText;
    public GameObject shopUI;
    public Transform shopContent;
    public GameObject itemPrefab;

    public PlayerHealth playerHealth;

    private GunManager gunManager;
    



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gunManager = FindObjectOfType<GunManager>();

        foreach (Product product in products)
        {
            GameObject item = Instantiate(itemPrefab, shopContent);

            product.itemRef = item;

            foreach (Transform child in item.transform)
            {
                if (child.gameObject.name == "Cost")
                {
                    child.gameObject.GetComponent<Text>().text = "$" + product.cost.ToString();
                }
                else if (child.gameObject.name == "Product Name")
                {
                    child.gameObject.GetComponent<Text>().text = product.name;
                }
                else if (child.gameObject.name == "Image")
                {
                    child.gameObject.GetComponent<Image>().sprite = product.image;
                }
            }

            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuyProduct(product);
            });
        }
    }

    public void BuyProduct(Product product)
    {
        if (money >= product.cost)
        {
            money -= product.cost;

            ApplyPurchase(product);
        }
    }

    public void ApplyPurchase(Product product)
    {
        switch (product.name)
        {
            case "Assult Rifle":
                gunManager.EquipNewGun(product.gunPrefab);
                
                break;

            case "Sniper Rifle":
                gunManager.EquipNewGun(product.gunPrefab);
                break;

            case "Max Ammo":
                gunManager.MaxAmmo();
                break;

            case "Heal":

                playerHealth.numberOfHeals++;
                
                break;

            default:
                Debug.Log("Purchase Unavailable");
                break;


        }
    }

    public void ToggleShop()
    {
        shopUI.SetActive(!shopUI.activeSelf);

        if (shopUI.activeSelf)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;                 
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;                  
    }

    private void OnGUI()
    {
        moneyText.text = "Money: " + money.ToString();
    }

    [System.Serializable]
    public class Product
    {
        public string name;
        public int cost;
        public Sprite image;
        public GameObject gunPrefab;
        

        

        [HideInInspector]
        public GameObject itemRef;
    }
}
