using UnityEngine;
using System;
using TMPro;


public class ShopItem : MonoBehaviour
{
    // public string name;

    public int basePrice;
    public int priceMultiplier = 2;
    public int price;
    public string type = "Normal";
    public string infoText;
    public static event Action<ShopItem> onItemBought;

    public bool bought = false;
    public bool stackable = true;

    public virtual void Buy()
    {
        Debug.Log($"ShopItem Bought: {name}");
        GameManager.Instance.RegisterItem(name); 
        GameManager.Instance.RemoveMoney(price); 
    }

    public virtual void Update()
    {
        int lvl = GameManager.Instance.GetItemCountByName(name);
        if (lvl >= 1) bought = true;

        price = basePrice * (int)Mathf.Pow(priceMultiplier, lvl);

        TextMeshProUGUI[] inputs = GetComponentsInChildren<TextMeshProUGUI>(true);
       
        foreach (var input in inputs)
        {
            if (input.gameObject.name == "PriceText")
            {
                input.text = price.ToString() + " €";
            }

            if (input.gameObject.name == "InfoText")
            {
                input.text = infoText + "\n" + "Lvl: " + lvl.ToString();
            }

            if (input.gameObject.name == "RetroText" && bought)
            {
                input.text = "Item Bought";
			}
			else if (input.gameObject.name == "RetroText" && !bought)
			{
				input.text = "Item Not Bought";
			}

			if (input.gameObject.name == "ConfText")
            {
                input.text = "Buy This Item?";
            } 
    
        }
    }
    public virtual void Start()
    {
        gameObject.SetActive(true); // Ensure the item is active at the start

        int lvl = GameManager.Instance.GetItemCountByName(name);
        if (lvl >= 1) bought = true;

        price = basePrice * (int)Mathf.Pow(priceMultiplier, lvl);

        TextMeshProUGUI[] inputs = GetComponentsInChildren<TextMeshProUGUI>(true);
       
        foreach (var input in inputs)
        {
            if (input.gameObject.name == "PriceText")
            {
                input.text = price.ToString() + " €";
            }

            if (input.gameObject.name == "InfoText")
            {
                input.text = infoText + "\n" + "Lvl: " + lvl.ToString();
            }

            if (input.gameObject.name == "RetroText")
            {
                input.text = "Item Bought";
            }

            if (input.gameObject.name == "ConfText")
            {
                input.text = "Buy This Item?";
            } 
    
        }
    }

    public virtual void SetPriceError()
    {
        TextMeshProUGUI[] inputs = GetComponentsInChildren<TextMeshProUGUI>(true);
       
        foreach (var input in inputs)
        {
            if (input.gameObject.name == "RetroText")
            {
                input.text = "Not enough coins";
            }
        }
    }

    public virtual void SetStackError()
    {
        TextMeshProUGUI[] inputs = GetComponentsInChildren<TextMeshProUGUI>(true);
       
        foreach (var input in inputs)
        {
            if (input.gameObject.name == "RetroText")
            {
                input.text = "Item can only be bought once";
            }
        }
    }
}
