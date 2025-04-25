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

        /*Vector3 position = new Vector3(0, 0, 0);
        xrOrigin.transform.position = position;

        xrOrigin.gameObject.SetActive(false);
        xrOrigin.gameObject.SetActive(true);

        Destroy(UI_ini);
        UI_game.SetActive(true);*/
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager no está en la escena.");
            return;
        }

        //GameManager.Instance.SetState(GameState.Playing);
        GameManager.Instance.SetState(GameState.Shop);
    }

}
/*
he pulsado el valor

NullReferenceException: Object reference not set to an instance of an object
StartGame.StartGameAction () (at Assets/Scripts/StartGame.cs:25)
UnityEngine.Events.InvokableCall.Invoke () (at <c3dca0e61d5249b79ac00f7b2c7a01ef>:0)
UnityEngine.Events.UnityEvent.Invoke () (at <c3dca0e61d5249b79ac00f7b2c7a01ef>:0)

NullReferenceException: Object reference not set to an instance of an object
UnityEngine.InputSystem.InputActionState.ApplyProcessors[TValue] (System.Int32 bindingIndex, TValue value, UnityEngine.InputSystem.InputControl`1[TValue] controlOfType) (at ./Library/PackageCache/com.unity.inputsystem@1.11.2/InputSystem/Actions/InputActionState.cs:2887)
*/
