using UnityEngine;
using System;

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
}
