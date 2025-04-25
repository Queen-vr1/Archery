using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ExitShop : MonoBehaviour
{

    public XRBaseInteractable interactable;
    public void Exit()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager no está en la escena.");
            return;
        }

        GameManager.Instance.SetState(GameState.Playing);
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelected);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelected);
    }

    void OnSelected(SelectEnterEventArgs args)
    {
        Debug.Log("Exit Shop");
        Exit();
    }

}
