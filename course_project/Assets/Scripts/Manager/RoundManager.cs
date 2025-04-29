using UnityEngine;
using System.Collections;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;

public class RoundManager : MonoBehaviour
{
    public float roundDuration = 60f;
    public int baseTargetScore = 30;

    private float timeRemaining;
    private int currentTarget;
	private int roundMoney = 0;

	private bool roundActive = false;

    public GameObject portal2level;
    public GameObject portal2shop;
    public Transform head;
    public XROrigin xrOrigin;
    public float spawnDistance = 2f;
    public bool isFinalLevel = false;

    private void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        //ScoreManager.Instance.ResetScore();
        //currentTarget = Mathf.RoundToInt(baseTargetScore * Mathf.Pow(1.5f, GameManager.Instance.CurrentLevel - 1));
        roundActive = true;

		//if (GameManager.Instance.CurrentLevel == 1)
		if (GameManager.Instance.CurrentLevel % 5 == 0)
		{
			ChallengeManager.Instance.StartChallenge();
		}

        timeRemaining = roundDuration;
		roundMoney = 0;

		//GameManager.Instance.SetState(GameState.Playing);
		StartCoroutine(RunRound());
    }

    private IEnumerator RunRound()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        EndRound();
    }

    private void EndRound()
    {
        roundActive = false;
        //int score = ScoreManager.Instance.TotalPoints;

        //if (score >= currentTarget)
        if (true)
        {
			//Debug.Log($"✅ Ronda superada con {score}/{currentTarget} puntos");
			if (GameManager.Instance == null)
			{
				Debug.LogError("GameManager no está en la escena.");
				return;
			}

			int bonus = 3 + GameManager.Instance.ChallengesCompleted + Mathf.FloorToInt(timeRemaining / 10f);
            GameManager.Instance.AddMoney(bonus + roundMoney);

            PortalTeleport portalTeleporter = portal2level.GetComponent<PortalTeleport>();
            if (portalTeleporter != null)
            {
                portalTeleporter.Activate(head, spawnDistance, GameState.Playing, -1f, 20f);
                if (!isFinalLevel)
                {
                    GameManager.Instance.NextLevel();
                }
                else
                {
                    GameManager.Instance.RepeatLevels();
                }
            }
            else
            {
                Debug.LogError("El portal no tiene el componente PortalTeleport.");
            }

            PortalTeleport portalTeleporterShop = portal2shop.GetComponent<PortalTeleport>();
            if (portalTeleporterShop != null)
            {
                portalTeleporterShop.Activate(head, spawnDistance, GameState.Shop, -1f, -20f);
            }
            else
            {
                Debug.LogError("El portal no tiene el componente PortalTeleport.");
            }
  
        }
        else
        {
            //Debug.Log($"❌ Ronda fallida: {score}/{currentTarget} puntos");
            GameManager.Instance.SetState(GameState.GameOver);
        }
    }

    public int GetTargetScore()
    {
        return currentTarget;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

	public int GetRoundMoney()
	{
		return roundMoney;
	}

	public void AddRoundMoney(int amount)
	{
		roundMoney += amount;
	}
}
