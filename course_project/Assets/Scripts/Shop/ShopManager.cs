using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Buy(ShopItem item)
    {
        if (item.bought)
        {
            Debug.Log("Shop Manager: Already bought.");
            return;
        }

        if (GameManager.Instance.Money >= item.price)
        {
            Debug.Log("Shop Manager: Buying item.");
            GameManager.Instance.RemoveMoney(item.price);
            item.Buy();
        }
        else
        {
            Debug.Log("Shop Manager: U broke, not enough coins.");
        }
    }
}
