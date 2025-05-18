using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int TotalPoints { get; private set; } = 0;
    public float Multiplier { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddPoints(int basePoints)
    {
        int finalPoints = Mathf.RoundToInt(basePoints * Multiplier);
        TotalPoints += finalPoints;
        Debug.Log($"+{finalPoints} puntos (x{Multiplier})");
    }

    public void AddFlatPoints(int points)
    {
        TotalPoints = Mathf.Max(0, TotalPoints + points);
        Debug.Log($"{points} puntos planos");
    }

    public void SetMultiplier(float newMultiplier)
    {
        Multiplier = newMultiplier;
    }

    public void ResetScore()
    {
        TotalPoints = 0;
        Multiplier = 1f;
    }

}
