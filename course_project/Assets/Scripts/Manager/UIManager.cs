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
	public TextMeshProUGUI challengeText;

	private void Start()
	{
		if (ChallengeManager.Instance != null && ChallengeManager.Instance.challengeActive)
		{
			ShowChallenge(ChallengeManager.Instance.GetChallengeName());
		}
		else
		{
			if (challengeText != null)
				challengeText.gameObject.SetActive(false);
		}
	}

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

	public void ShowChallenge(string challengeName)
	{
		StartCoroutine(ShowChallengeCoroutine(challengeName));
	}

	private IEnumerator ShowChallengeCoroutine(string challengeName)
	{
		if (challengeText != null)
		{
			challengeText.text = $"Challenge!\n{challengeName}";
			challengeText.gameObject.SetActive(true);
			yield return new WaitForSeconds(2f);
			challengeText.gameObject.SetActive(false);
		}
	}
}
