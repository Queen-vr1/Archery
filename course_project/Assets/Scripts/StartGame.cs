using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using System.Collections.Generic;

public class StartGame : MonoBehaviour
{
    public GameObject UI_ini;
    public GameObject UI_game;
    public XROrigin xrOrigin;

    public void StartGameAction()
    {

        Vector3 position = new Vector3(0, 0, 0);
        xrOrigin.transform.position = position;

        xrOrigin.gameObject.SetActive(false);
        xrOrigin.gameObject.SetActive(true);

        Destroy(UI_ini);
        UI_game.SetActive(true);
    }

}
