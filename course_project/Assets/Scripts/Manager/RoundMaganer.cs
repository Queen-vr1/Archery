using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    public float roundDuration = 60f;
    public int baseTargetScore = 30;

    private float timeRemaining;
    private int currentTarget;

    private bool roundActive = false;

    public void StartRound()
    {
        ScoreManager.Instance.ResetScore();
        timeRemaining = roundDuration;
        currentTarget = Mathf.RoundToInt(baseTargetScore * Mathf.Pow(1.5f, GameManager.Instance.CurrentLevel - 1));
        roundActive = true;

        GameManager.Instance.SetState(GameState.Playing);
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
        int score = ScoreManager.Instance.TotalPoints;

        if (score >= currentTarget)
        {
            Debug.Log($"✅ Ronda superada con {score}/{currentTarget} puntos");

            int bonus = 3 + GameManager.Instance.ChallengesCompleted + Mathf.FloorToInt(timeRemaining / 10f);
            GameManager.Instance.AddMoney(bonus);
            GameManager.Instance.SetState(GameState.Shop);
        }
        else
        {
            Debug.Log($"❌ Ronda fallida: {score}/{currentTarget} puntos");
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
}
