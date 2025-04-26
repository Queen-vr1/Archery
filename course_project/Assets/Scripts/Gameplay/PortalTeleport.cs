using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTeleport : MonoBehaviour
{
	private string sceneToLoad;  
	private GameState nextState;

	private bool readyToTeleport = false;

	public void SetupPortal(GameState gameState)
	{
		nextState = gameState;
		readyToTeleport = true;
		Debug.Log("Portal is ready to teleport to: " + nextState);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (readyToTeleport && other.CompareTag("Player"))
		{
			Debug.Log("Player entered the portal.");
			Teleport();
		}
		else
		{
			Debug.Log("Player did not enter the portal. Tag: " + other.tag);
			
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
