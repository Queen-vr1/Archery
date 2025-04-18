using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ForceGrabOnStart : MonoBehaviour
{
    public XRBaseInteractor leftHandInteractor;
    public XRGrabInteractable bowInteractable;

    void Start()
    {
        // Forzar el agarre del arco al iniciar
        if (leftHandInteractor && bowInteractable)
        {
            leftHandInteractor.interactionManager.SelectEnter(leftHandInteractor, bowInteractable);
        }
    }
}
