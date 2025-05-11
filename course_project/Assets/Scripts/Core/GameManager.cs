using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.MainMenu;
    public HandicapType CurrentHandicap { get; private set; } = HandicapType.None;
    public HandicapState HandicapState { get; private set; } = new HandicapState();

    public int CurrentLevel { get; private set; } = 1;
    public int Money { get; private set; } = 0;
    public int ChallengesCompleted { get; private set; } = 0;
    
    // Things for the shop
    public List<string> itemsBought = new List<string>();

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


    public void GenerateHandicap()
    {
        if (CurrentLevel >= 1)
        {
            // Enum.GetValues devuelve todos los valores, excluye "None"
            var values = System.Enum.GetValues(typeof(HandicapType)).Cast<HandicapType>().Where(h => h != HandicapType.None).ToArray();
            CurrentHandicap = values[Random.Range(0, values.Length)];
            HandicapState.Apply(CurrentHandicap);
            Debug.Log("Handicap aplicado: " + CurrentHandicap);
        }
        else
        {
            CurrentHandicap = HandicapType.None;
        }
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
		Money = Mathf.Max(Money, 0);
		Debug.Log($"Dinero actual: €{Money}");
    }
    public void NextLevel()
    {
        CurrentLevel++;
        // SetState(GameState.Playing);
    }

    public void CompleteChallenge()
    {
        ChallengesCompleted++;
    }

    public void RepeatLevels()
    {
        CurrentLevel = 1;
    }

    public void ResetProgress()
    {
        CurrentLevel = 1;
        Money = 0;
        ChallengesCompleted = 0;
    }

    
    public void RegisterItem(string item)
    {
        itemsBought.Add(item);
        Debug.Log($"ShopRegister: Item added: {item}");

    }

    public void EquipArrow(string arrow)
    {
        string activeName = "";
        bool arrowActive = GetActiveArrow(ref activeName);
        if (arrowActive)
        {
            // Deactivate the previous arrow
            itemsBought.Remove(activeName);
            // Add the new arrow to the list
            itemsBought.Add(arrow);      
            Debug.Log("Active arrow replaced: " + activeName + " with " + arrow);

        } else {
            // Add the new arrow to the list
            itemsBought.Add(arrow); 
            Debug.Log("Active arrow: " + arrow);
        }

    }

    public int GetItemCountByName(string itemName)
    {
        return itemsBought.Count(name => name == itemName);
    }

    public bool GetActiveArrow(ref string arrowName) 
    {
        var arrows = itemsBought.Where(name => name.StartsWith("Arrow_"));

        if (!arrows.Any())
        {
            Debug.Log("No arrow upgrades found.");
            return false;
        }

        var grouped = arrows.GroupBy(name => name)
                            .Select(group => new { Name = group.Key, Count = group.Count() })
                            .Where(g => g.Count == 2) // Solo los que aparecen 2 veces
                            .ToList();

        if (grouped.Count == 0)
        {
            Debug.Log("No active arrows found.");
            return false;
        }
        else
        {
            arrowName = grouped[0].Name;
            Debug.Log($"Active arrow found: {arrowName}");
            return true;
        }
    }
   
}

