using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }
    public int CurrentLevel { get; set; } = 1;
    public int Money { get; set; } = 0;

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

        // Aquí puedes reaccionar al cambio de estado
        Debug.Log("Estado del juego: " + newState);
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        Debug.Log("Dinero: " + Money);
    }

    public void ResetGame()
    {
        Money = 0;
        CurrentLevel = 1;
        SetState(GameState.MainMenu);
    }
}
