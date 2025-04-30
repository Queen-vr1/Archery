using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
	public static ChallengeManager Instance { get; private set; }

	public bool challengeActive { get; private set; } = false;
	private ChallengeType currentChallenge;
	private int dsf => GameManager.Instance.ChallengesCompleted;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void StartChallenge()
	{
		challengeActive = true;

		// random challenge
		currentChallenge = (ChallengeType)Random.Range(0, System.Enum.GetValues(typeof(ChallengeType)).Length);
		Debug.Log($"Challenge: {currentChallenge}");

		// apply challenge
		switch (currentChallenge)
		{
			case ChallengeType.ReduceTime:
				ApplyReduceTime();
				break;

			case ChallengeType.IncreaseTargetScore:
				ApplyIncreaseTargetScore();
				break;

			case ChallengeType.MoneyDrain:
				StartCoroutine(ApplyMoneyDrain());
				break;

			case ChallengeType.ReduceMultiplier:
				StartCoroutine(ApplyReduceMultiplier());
				break;

			case ChallengeType.DeadBalloons:
				ApplyDeadBalloons();
				break;
		}
	}

	public void EndChallenge()
	{
		challengeActive = false;
	}

	// Challenge types
	public enum ChallengeType
	{
		ReduceTime,
		IncreaseTargetScore,
		MoneyDrain,
		ReduceMultiplier,
		DeadBalloons
	}

	public string GetChallengeName()
	{
		switch (currentChallenge)
		{
			case ChallengeType.ReduceTime:
				return "Reduction of time";

			case ChallengeType.IncreaseTargetScore:
				return "A higher target score";

			case ChallengeType.MoneyDrain:
				return "Money drain";

			case ChallengeType.ReduceMultiplier:
				return "Multiplier reduction";

			case ChallengeType.DeadBalloons:
				return "Dead balloons";

			default:
				return "No challenge";
		}
	}


	// Challenge functions
	private void ApplyReduceTime()
	{
		RoundManager roundManager = FindObjectOfType<RoundManager>();
		if (roundManager != null)
		{
			float reduction = 0.2f + 0.05f * dsf;
			float newDuration = roundManager.roundDuration * (1 - reduction);
			roundManager.roundDuration = Mathf.Max(20f, newDuration);
			Debug.Log($"Reduction: {reduction * 100}%, Time: {roundManager.roundDuration} seconds");
		}
	}

	private void ApplyIncreaseTargetScore()
	{
		RoundManager roundManager = FindObjectOfType<RoundManager>();
		if (roundManager != null)
		{
			float increase = 2f + 0.5f * dsf;
			int target = roundManager.GetCurrentTarget();
			int newTarget = Mathf.RoundToInt(target * increase);
			roundManager.SetCurrentTarget(newTarget);
			Debug.Log($"Increase: {increase}, Old Target: {target}, New Target: {newTarget}");
		}
	}

	private IEnumerator ApplyMoneyDrain()
	{
		int dsf = GameManager.Instance.ChallengesCompleted;
		float time = Mathf.Max(1f, 10f - dsf);

		while (challengeActive)
		{
			yield return new WaitForSeconds(time);

			int money = GameManager.Instance.Money;
			if (money > 0)
			{
				int drain = Mathf.RoundToInt(0.1f * money);
				GameManager.Instance.RemoveMoney(drain);
				Debug.Log($"Money drain: {drain}€");
			}
		}
	}

	private IEnumerator ApplyReduceMultiplier()
	{
		int dsf = GameManager.Instance.ChallengesCompleted;
		float time = Mathf.Max(1f, 10f - dsf);
		float reduction = 1f + 2f * dsf;

		while (challengeActive)
		{
			yield return new WaitForSeconds(time);

			if (ScoreManager.Instance != null)
			{
				float newMultiplier = Mathf.Max(1f, ScoreManager.Instance.Multiplier - reduction);
				ScoreManager.Instance.SetMultiplier(newMultiplier);
				Debug.Log($"Reduction: {reduction}, New Multiplier: {newMultiplier}");
			}
		}
	}

	private void ApplyDeadBalloons()
	{
		float count = 0.3f + 0.3f * dsf;

		SpawmerBalloon spawner = FindObjectOfType<SpawmerBalloon>();
		if (spawner != null)
		{
			BalloonType balloon = spawner.balloonTypes.Find(bt => bt.prefab.name == "Dead_Balloon");
			if (balloon != null)
			{
				balloon.weight = Mathf.Min(balloon.weight + count, 0.8f);
				Debug.Log($"DeadBalloon weight: {balloon.weight}");
			}
			else
			{
				Debug.LogError("Prefab not found");
			}
		}
	}
}
