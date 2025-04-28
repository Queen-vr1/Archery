using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PortalTeleport : MonoBehaviour
{ 
	public GameState nextState;
	public bool readyToTeleport = false;
	public float positionPortal = -1f;
	private Transform head;
    private float spawnDistance;
    private GameState gameState;
    private float disableDelay;

	public void Activate(Transform playerHead, float distance, GameState state, float delay, float positionPortal_ = -1)
    {
        head = playerHead;
        spawnDistance = distance;
        gameState = state;
        disableDelay = delay;
		positionPortal = positionPortal_;

        gameObject.SetActive(true);



        if (head != null)
        {

			if (positionPortal != -1){
				Vector3 forward = new Vector3(head.forward.x, 0, head.forward.z).normalized;
				Vector3 offsetLeft = Quaternion.Euler(0, positionPortal, 0) * forward; 
				transform.position = head.position + offsetLeft * spawnDistance;
			}
			else{
            	transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
			}

            transform.LookAt(new Vector3(head.position.x, transform.position.y, head.position.z));
            transform.forward *= -1;
        }

        PortalTeleport portalTeleporter = GetComponent<PortalTeleport>();
        if (portalTeleporter != null)
        {
            portalTeleporter.SetupPortal(gameState);
        }

		if (disableDelay > 0)
		{
        	StartCoroutine(DisableAfterSeconds());
		}
    }

	private IEnumerator DisableAfterSeconds()
	{
		yield return new WaitForSeconds(disableDelay);
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
	}

	public void SetupPortal(GameState gameState)
	{
		nextState = gameState;
		readyToTeleport = true;
		Debug.Log("Portal is ready to teleport to: " + nextState);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnTriggerEnter called. Other: " + other.name);
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
		else
		{
			Debug.LogError("GameManager no está en la escena.");
		}
	}
}
