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

    private void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        //ScoreManager.Instance.ResetScore();
        timeRemaining = roundDuration;
        //currentTarget = Mathf.RoundToInt(baseTargetScore * Mathf.Pow(1.5f, GameManager.Instance.CurrentLevel - 1));
        roundActive = true;

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
        
           if (portal2level != null)
            {
                portal2level.SetActive(true);

                Vector3 forward = new Vector3(head.forward.x, 0, head.forward.z).normalized;

                Vector3 offsetRight = Quaternion.Euler(0, 20, 0) * forward; 
                portal2level.transform.position = head.position + offsetRight * spawnDistance;

                portal2level.transform.LookAt(new Vector3(head.position.x, portal2level.transform.position.y, head.position.z));
                portal2level.transform.forward *= -1;

                GameManager.Instance.NextLevel();
            }

            if (portal2shop != null)
            {
                portal2shop.SetActive(true);

                Vector3 forward = new Vector3(head.forward.x, 0, head.forward.z).normalized;
               
                Vector3 offsetLeft = Quaternion.Euler(0, -20, 0) * forward; 
                portal2shop.transform.position = head.position + offsetLeft * spawnDistance;

                portal2shop.transform.LookAt(new Vector3(head.position.x, portal2shop.transform.position.y, head.position.z));
                portal2shop.transform.forward *= -1; 

                PortalTeleport portalTeleporter = portal2shop.GetComponent<PortalTeleport>();
                if (portalTeleporter != null)
                {
                    portalTeleporter.SetupPortal(GameState.Shop);
                }
            }
            //GameManager.Instance.NextLevel();
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
