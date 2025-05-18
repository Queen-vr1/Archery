using UnityEngine;
using System.Collections;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;

public class RoundManager : MonoBehaviour
{
    public float roundDuration = 60f;
    public int baseTargetScore = 10;

    public float timeRemaining { get; private set; }
    private int currentTarget;
	private int roundMoney = 0;

	private bool roundActive = false;

    public GameObject portal2level;
    public GameObject portal2shop;
	public GameObject portalHome;
	public Transform head;
    public float spawnDistance = 2f;
    public bool isFinalLevel = false;

    [SerializeField] private SpawmerBalloon spawmerBalloon;


    private void Start()
    {
        GameManager.Instance.GenerateHandicap();
        StartRound();
        if (spawmerBalloon == null)
        {
            Debug.LogError("SpawmerBalloon no está asignado. Buscando en el objeto actual.");
            spawmerBalloon = GetComponent<SpawmerBalloon>();
        }
        int penaltyBalloons = GameManager.Instance.HandicapState.FewerBalloons;
		int upgradeBalloons = GameManager.Instance.UpgradeState.Add_Balloon;
		float penaltySize = GameManager.Instance.HandicapState.SizePenalty;
		float upgradeSize = GameManager.Instance.UpgradeState.Balloon_Size;
		Debug.Log($"PenaltyBalloons: {penaltyBalloons}, UpgradeBalloons: {upgradeBalloons}");
		Debug.Log($"PenaltySize: {penaltySize}, UpgradeSize: {upgradeSize}");

		spawmerBalloon.initializateAndSpawn(penaltyBalloons - upgradeBalloons, penaltySize - upgradeSize);
	}

    public void StartRound()
    {
        ScoreManager.Instance.ResetScore();
        currentTarget = Mathf.RoundToInt(baseTargetScore * Mathf.Pow(1.5f, GameManager.Instance.CurrentLevel - 1));
        currentTarget += (int)GameManager.Instance.HandicapState.TargetPoints;

        roundActive = true;

		if (GameManager.Instance.CurrentLevel % 5 == 0)
		{
			ChallengeManager.Instance.StartChallenge();
		}

        timeRemaining = roundDuration - GameManager.Instance.HandicapState.LessTime + GameManager.Instance.UpgradeState.Time;
		if (timeRemaining < 10)
        {
            timeRemaining = 10;
        }
		roundMoney = 0;

		ScoreManager.Instance.AddPoints(GameManager.Instance.UpgradeState.Init_Points);
		ScoreManager.Instance.SetMultiplier(GameManager.Instance.UpgradeState.Power_Up);
        
        GameManager.Instance.ResetQuiver();

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

		if (ScoreManager.Instance.TotalPoints >= currentTarget)
		// if (true)
        {
			//Debug.Log($"✅ Ronda superada con {score}/{currentTarget} puntos");
			if (GameManager.Instance == null)
			{
				Debug.LogError("GameManager no está en la escena.");
				return;
			}

			if (ChallengeManager.Instance.challengeActive)
			{
				GameManager.Instance.CompleteChallenge();
				ChallengeManager.Instance.EndChallenge();
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
			PortalTeleport portalTeleporterHome = portalHome.GetComponent<PortalTeleport>();
			if (portalTeleporterHome != null)
			{
				portalTeleporterHome.Activate(head, spawnDistance, GameState.MainMenu, -1f, -20f);
			}
			else
			{
				Debug.LogError("El portal no tiene el componente PortalTeleport.");
			}
			//Debug.Log($"❌ Ronda fallida: {score}/{currentTarget} puntos");
			// GameManager.Instance.SetState(GameState.GameOver);
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

	public int GetCurrentTarget()
	{
		return currentTarget;
	}

	public void SetCurrentTarget(int target)
	{
		currentTarget = target;
	}
}
