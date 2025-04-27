using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Money : MonoBehaviour
{
	public TextMeshProUGUI moneyText;

    void Update()
    {
		if (GameManager.Instance != null)
		{
			moneyText.text = $"{GameManager.Instance.Money}€";
		}
	}
}
