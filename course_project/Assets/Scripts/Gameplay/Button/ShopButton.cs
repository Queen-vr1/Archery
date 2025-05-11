using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

public class ShopButton : MonoBehaviour
{
    public XROrigin xrOrigin;
    public GameObject portal;

    public Transform head;
    public float spawnDistance = 2f;

    public AudioSource clickSound;

    public void StartShopAction()
    {
        clickSound.Play();
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager no está en la escena.");
            return;
        }

        PortalTeleport portalTeleporter = portal.GetComponent<PortalTeleport>();
        if (portalTeleporter != null)
        {
            portalTeleporter.Activate(head, spawnDistance, GameState.Shop, 5f, -1);
        }
        else
        {
            Debug.LogError("El portal no tiene el componente PortalTeleport.");
        }
    }

}
