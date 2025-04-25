using UnityEngine;
using System;
using TMPro;


public class ShopItem : MonoBehaviour
{
    // public string name;
    public int price;
    public bool bought = false;

    public static event Action<ShopItem> onItemBought;

    public virtual void Buy()
    {
        if (bought) return;

        bought = true;
        Debug.Log($"ShopItem Bought: {name}");
        onItemBought?.Invoke(this);  // Notify subscribers that an item has been bought
        gameObject.SetActive(false); // Remove the item from the shop
    }

    public void Start()
    {
        Debug.Log("Hol???????a");
        TextMeshProUGUI[] inputs = GetComponentsInChildren<TextMeshProUGUI>(true);
        Debug.Log("Encontrados: " + inputs.Length);

        foreach (var input in inputs)
        {
            Debug.Log("Input field found: " + input.gameObject.name);
            if (input.gameObject.name == "PriceText")
            {
                input.text = price.ToString() + " €";
            }
        }
    }
}
