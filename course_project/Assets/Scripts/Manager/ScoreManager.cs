using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Aplica multiplicador según el nivel actual
    public void RewardForBalloon(Balloon balloon)
    {
        int baseValue = balloon.GetReward();
        int level = GameManager.Instance.CurrentLevel;

        int finalReward = baseValue * level;

        // Añadir dinero al total
        GameManager.Instance.Money += finalReward;

        // Mostrar en consola o UI
        Debug.Log($"+{finalReward} monedas por globo (base {baseValue}, nivel {level})");
    }
}
