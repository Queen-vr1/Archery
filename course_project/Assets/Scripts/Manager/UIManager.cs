using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using Unity.VisualScripting.Dependencies.Sqlite;

public class UIManager : MonoBehaviour
{
	public TextMeshProUGUI levelText;
	public TextMeshProUGUI moneyText;
	public TextMeshProUGUI timeText;
	public TextMeshProUGUI pointsText;
	public TextMeshProUGUI multiplierText;
	public TextMeshProUGUI challengeText;
	public TextMeshProUGUI arrowCounter;
	public TextMeshProUGUI targetPointsText;

	private int lastLevel = -1;
	private int lastMoney = -1;
	private int lastPoints = -1;
	private float lastMultiplier = -1f;
	private int lastArrowCount = -1;
	private int lastTarget = -1;
	private Color originalTimeColor;

	private float animationTime = 0.5f;
	private float scale = 1.2f;

	public AudioSource clockSound;

	private void Start()
	{
		if (timeText != null)
		{
			originalTimeColor = timeText.color;
		}

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
			if (GameManager.Instance.CurrentLevel != lastLevel)
			{
				levelText.text = $"{GameManager.Instance.CurrentLevel}";
				AnimateText(levelText);
				lastLevel = GameManager.Instance.CurrentLevel;
			}

			if (GameManager.Instance.Money != lastMoney)
			{
				moneyText.text = $"{GameManager.Instance.Money}€";
				AnimateText(moneyText);
				lastMoney = GameManager.Instance.Money;
			}

			
			int arrowCount = GameManager.Instance.Quiver;
			if (arrowCount != lastArrowCount)
			{
				arrowCounter.text = $"{arrowCount}";
				AnimateText(arrowCounter);
				lastArrowCount = arrowCount;
			}
			
		}

		if (RoundManagerInstance() != null)
		{
			float time = Mathf.Max(0, RoundManagerInstance().GetTimeRemaining());
			int minutes = Mathf.FloorToInt(time / 60f);
			int seconds = Mathf.FloorToInt(time % 60f);
			timeText.text = $"{minutes:00}:{seconds:00}";

			if (time <= 10f)
			{
				if (!clockSound.isPlaying)
				{
					clockSound.Play();
				} else if (time <= 0f)
				{
					clockSound.Stop();
				}
				timeText.color = new Color(0.6f, 0f, 0f);
			}
			else
			{
				timeText.color = originalTimeColor;
			}

			
			int target = RoundManagerInstance().GetTargetScore();
			if (target != lastTarget)
			{
				targetPointsText.text = $"{target}";
				AnimateText(targetPointsText);
				lastTarget = target;
			}
		}

		if (ScoreManager.Instance != null)
		{
			if (ScoreManager.Instance.TotalPoints != lastPoints)
			{
				pointsText.text = $"{ScoreManager.Instance.TotalPoints}";
				AnimateText(pointsText);
				lastPoints = ScoreManager.Instance.TotalPoints;
			}

			if (ScoreManager.Instance.Multiplier != lastMultiplier)
			{
				multiplierText.text = $"{ScoreManager.Instance.Multiplier:0.0}";
				AnimateText(multiplierText);
				lastMultiplier = ScoreManager.Instance.Multiplier;
			}
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

	private void AnimateText(TextMeshProUGUI text)
	{
		StartCoroutine(AnimateTextCoroutine(text));
	}

	private IEnumerator AnimateTextCoroutine(TextMeshProUGUI text)
	{
		Vector3 originalScale = text.transform.localScale;
		Vector3 targetScale = originalScale * scale;

		float elapsed = 0f;
		while (elapsed < animationTime)
		{
			float t = elapsed / animationTime;
			text.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
			elapsed += Time.deltaTime;
			yield return null;
		}
		text.transform.localScale = targetScale;

		elapsed = 0f;
		while (elapsed < animationTime)
		{
			float t = elapsed / animationTime;
			text.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
			elapsed += Time.deltaTime;
			yield return null;
		}
		text.transform.localScale = originalScale;
	}
}
