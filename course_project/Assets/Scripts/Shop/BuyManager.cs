using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyManager : MonoBehaviour
{
	[System.Serializable]
	public class Item
	{
		public GameObject itemPrefab;
		public Button buyButton;
		public GameObject buyPanel;
		public Button yesButton;
		public Button noButton;
		public GameObject retroPanel;
	}

	public List<Item> items = new List<Item>();
	public PanelManager panelManager;

	public AudioSource buySound;
	public AudioSource clickSound;


	void Start()
	{
		Debug.Log("Start");
		foreach (var item in items)
		{
			item.buyButton.onClick.AddListener(() => OnItemSelected(item));
			item.yesButton.onClick.AddListener(() => OnYesButtonClicked(item));
			item.noButton.onClick.AddListener(() => OnNoButtonClicked(item));

			item.buyPanel.SetActive(false);
			item.retroPanel.SetActive(false);
		}
	}

	void OnItemSelected(Item item)
	{
		clickSound.Play();
		Debug.Log("Seleccionado");
		foreach (var pair in panelManager.pairs)
		{
			pair.panel.SetActive(false);
		}

		ShopItem shopItem = item.itemPrefab.GetComponent<ShopItem>(); // Only the case for the gems
		if (shopItem.bought && !shopItem.stackable)
		{
			shopItem.SetStackError();
			item.retroPanel.SetActive(true);
			StartCoroutine(HideRetroPanel(item, false));
			return;
		}
		
		item.buyPanel.SetActive(true);

	}

	void OnYesButtonClicked(Item item)
	{
		bool bought = false;
		Debug.Log("Yes button clicked.");
		clickSound.Play();
		ShopItem shopItem = item.itemPrefab.GetComponent<ShopItem>();
		if (GameManager.Instance.Money >= shopItem.price || shopItem.bought && shopItem.type == "Arrow")
        {
            Debug.Log("Shop Manager: Buying item.");
            shopItem.Buy();
			bought = true;
			if (shopItem.type != "Arrow")
			{
				buySound.Play();
			}
		}
        else
        {
			shopItem.SetPriceError();
            Debug.Log("Shop Manager: U broke, not enough coins.");
        }
		item.buyPanel.SetActive(false);

		item.retroPanel.SetActive(true);
		StartCoroutine(HideRetroPanel(item, bought));
	}

	void OnNoButtonClicked(Item item)
	{
		clickSound.Play();
		Debug.Log("Bye");
		item.buyPanel.SetActive(false);
	}

	IEnumerator HideRetroPanel(Item item, bool bought)
	{
		yield return new WaitForSeconds(1f);
		item.retroPanel.SetActive(false);
		ShopItem shopItem = item.itemPrefab.GetComponent<ShopItem>();
		if (shopItem.type != "Arrow" && bought)
		{
			item.itemPrefab.SetActive(false);
		}
	}
	
}
