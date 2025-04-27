using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

public class StartGame : MonoBehaviour
{
    public GameObject UI_ini;
    public GameObject UI_game;
    public XROrigin xrOrigin;
    public GameObject portal;

    public Transform head;
    public float spawnDistance = 2f;

    public void StartGameAction()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager no está en la escena.");
            return;
        }

        if (portal != null)
        {
            portal.SetActive(true);
            portal.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
            portal.transform.LookAt(new Vector3(head.position.x, portal.transform.position.y, head.position.z));
            portal.transform.forward *= -1;

            PortalTeleport portalTeleporter = portal.GetComponent<PortalTeleport>();
            if (portalTeleporter != null)
            {
                //portalTeleporter.SetupPortal(GameState.Shop);
                portalTeleporter.SetupPortal(GameState.Playing);
            }

            StartCoroutine(DisablePortalAfterSeconds(5f));
        }
        //GameManager.Instance.SetState(GameState.Playing);
        // GameManager.Instance.SetState(GameState.Shop);
    }

    private IEnumerator DisablePortalAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (portal != null)
        {
            portal.SetActive(false);
        }
    }

}
