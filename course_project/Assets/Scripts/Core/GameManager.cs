using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.MainMenu;

    public int CurrentLevel { get; private set; } = 1;
    public int Money { get; private set; } = 0;
    public int ChallengesCompleted { get; private set; } = 0;

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

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("Estado del juego: " + newState);
        SceneController.Instance.LoadSceneForState(newState);
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        Debug.Log($"Dinero actual: €{Money}");
    }

    public void NextLevel()
    {
        CurrentLevel++;
        SetState(GameState.Playing);
    }

    public void CompleteChallenge()
    {
        ChallengesCompleted++;
    }

    public void ResetProgress()
    {
        CurrentLevel = 1;
        Money = 0;
        ChallengesCompleted = 0;
    }
}
