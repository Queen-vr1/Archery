using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
	public TextMeshProUGUI levelText;
	public TextMeshProUGUI moneyText;
	public TextMeshProUGUI timeText;
	public TextMeshProUGUI pointsText;
	public TextMeshProUGUI multiplierText;

	private void Update()
	{
		if (GameManager.Instance != null)
		{
			levelText.text = $"{GameManager.Instance.CurrentLevel}";
			moneyText.text = $"{GameManager.Instance.Money}€";
		}

		if (RoundManagerInstance() != null)
		{
			float time = Mathf.Max(0, RoundManagerInstance().GetTimeRemaining());
			int minutes = Mathf.FloorToInt(time / 60f);
			int seconds = Mathf.FloorToInt(time % 60f);
			timeText.text = $"{minutes:00}:{seconds:00}";
		}

		if (ScoreManager.Instance != null)
		{
			pointsText.text = $"{ScoreManager.Instance.TotalPoints}";
			multiplierText.text = $"{ScoreManager.Instance.Multiplier:0.0}";
		}
	}

	private RoundManager RoundManagerInstance()
	{
		return FindObjectOfType<RoundManager>();
	}
}
