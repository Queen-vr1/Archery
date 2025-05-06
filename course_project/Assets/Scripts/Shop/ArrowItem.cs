using UnityEngine;
using System;
using TMPro;


public class ArrowItem : ShopItem
{
   
    bool active = false;
    public static event Action<ArrowItem> onArrowEquip;

    public override void Buy()
    {
        Debug.Log($"ShopItem Bought: {name}");

        if (!bought){
            GameManager.Instance.RegisterItem(name); 
            GameManager.Instance.RemoveMoney(price);
        }

        GameManager.Instance.EquipArrow(name);  // Notify subscribers that an item has been equipped
												// The arrow upgrades always stay on the shop

		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.useGravity = true;
		}
	}

    public override void Start()
    {
        gameObject.SetActive(true); // Ensure the item is active at the start

        int lvl = GameManager.Instance.GetItemCountByName(name);

        if (lvl >= 1) bought = true;
        if (lvl == 2) active = true;

        type = "Arrow";

        price = basePrice;

        TextMeshProUGUI[] inputs = GetComponentsInChildren<TextMeshProUGUI>(true);
       
        foreach (var input in inputs)
        {
            if (input.gameObject.name == "PriceText" && !bought)
            {
                input.text = price.ToString() + " €";
            } 
            else if (input.gameObject.name == "PriceText" && bought)
            {
                input.text = "Adquired";
            }

            if (input.gameObject.name == "InfoText" && !bought)
            {
                input.text = infoText + "\n";

            }  else if (input.gameObject.name == "InfoText" && bought)
            {
                input.text = infoText + "\n" + "Equiped: " + active.ToString();
            }

            if (input.gameObject.name == "ConfText" && !bought)
            {
                input.text = "Buy This Arrow?";
            } 
            else if (input.gameObject.name == "ConfText" && bought)
            {
                input.text = "Equip This Arrow?";
            } 

            if (input.gameObject.name == "RetroText")
            {
                input.text = "Item Equipped";
            }

        }
    }
}
