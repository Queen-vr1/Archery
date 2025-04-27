using UnityEngine;
using System;
using TMPro;


public class ShopItem : MonoBehaviour
{
    // public string name;

    public int basePrice;
    public int priceMultiplier = 2;
    public int price;
    public string infoText;
    public static event Action<ShopItem> onItemBought;

    public virtual void Buy()
    {
        Debug.Log($"ShopItem Bought: {name}");
        onItemBought?.Invoke(this);  // Notify subscribers that an item has been bought
        gameObject.SetActive(false); // Remove the item from the shop
    }

    public void Start()
    {
        gameObject.SetActive(true); // Ensure the item is active at the start

        int lvl = GameManager.Instance.GetItemCountByName(name);

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
        }
    }
}
