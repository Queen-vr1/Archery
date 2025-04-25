using UnityEngine;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.MainMenu;

    public int CurrentLevel { get; private set; } = 1;
    public int Money { get; private set; } = 0;
    public int ChallengesCompleted { get; private set; } = 0;

    // Things for the shop
    public List<ShopItem> itemsBought = new List<ShopItem>();

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
    
    public void RemoveMoney(int amount)
    {
        Money -= amount;
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

    // Things for the shop
    void OnEnable()
    {
        ShopItem.onItemBought += RegisterItem;
    }

    void OnDisable()
    {
        ShopItem.onItemBought -= RegisterItem;
    }

    void RegisterItem(ShopItem item)
    {
        itemsBought.Add(item);
        Debug.Log($"ShopRegister: Item added: {item.name}");

    }
}
