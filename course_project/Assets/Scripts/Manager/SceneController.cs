using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

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

    public void LoadSceneForState(GameState state)
    {
        string sceneToLoad = GetSceneName(state);
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Estado sin escena asignada: " + state);
        }
    }

    private string GetSceneName(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                return "Start";

            case GameState.Playing:
                return $"Level_{GameManager.Instance.CurrentLevel}";

            case GameState.GameOver:
                return "GameOver";

            case GameState.Shop:
                return "Shop";

            default:
                return null;
        }
    }
}
