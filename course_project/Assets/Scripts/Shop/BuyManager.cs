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
	}

	public List<Item> items = new List<Item>();
	public PanelManager panelManager;

	void Start()
	{
		Debug.Log("Start");
		foreach (var item in items)
		{
			item.buyButton.onClick.AddListener(() => OnItemSelected(item));
			item.yesButton.onClick.AddListener(() => OnYesButtonClicked(item));
			item.noButton.onClick.AddListener(() => OnNoButtonClicked(item));

			item.buyPanel.SetActive(false);
		}
	}

	void OnItemSelected(Item item)
	{
		Debug.Log("Seleccionado");
		foreach (var pair in panelManager.pairs)
		{
			pair.panel.SetActive(false);
		}

		item.buyPanel.SetActive(true);
	}

	void OnYesButtonClicked(Item item)
	{
		Debug.Log("Holi");

		// Pongo que se compre aqui por ahora 
		ShopManager.Instance.Buy(item.itemPrefab.GetComponent<ShopItem>());

		item.buyPanel.SetActive(false);
	}

	void OnNoButtonClicked(Item item)
	{
		Debug.Log("Bye");
		item.buyPanel.SetActive(false);
	}
}
