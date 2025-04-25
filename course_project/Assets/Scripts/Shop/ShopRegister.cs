using System.Collections.Generic;
using UnityEngine;

// Fuse with the game manager


public class ShopRegister : MonoBehaviour
{
    public List<ShopItem> itemsBought = new List<ShopItem>();

    void OnEnable()
    {
        ShopItem.onItemBought += RegisterItem;
    }

    void OnDisable()
    {
        ShopItem.onItemBought -= RegisterItem;
    }

    void RegisterItem(ShopItem item)
    {
        itemsBought.Add(item);
        Debug.Log($"ShopRegister: Item added: {item.name}");

    }
}
