using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTeleporter : MonoBehaviour
{
    private string sceneToLoad;  // Ahora es privado
    private GameState nextState;

    private bool readyToTeleport = false; // Solo se teletransporta si está listo

    public void SetupPortal(GameState gameState)
    {
        nextState = gameState;
        readyToTeleport = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (readyToTeleport && other.CompareTag("Player"))
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(nextState);
        }
    }
}
