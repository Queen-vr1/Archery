using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttachPointDebugger : MonoBehaviour
{
    public XRBaseInteractor interactor;
    public XRGrabInteractable grabInteractable;

    void Update()
    {
        if (interactor && grabInteractable && interactor.selectTarget == grabInteractable)
        {
            // Fuerza a actualizar la posición basada en el nuevo AttachPoint
            grabInteractable.transform.position = interactor.attachTransform.position;
            grabInteractable.transform.rotation = interactor.attachTransform.rotation;
        }
    }
}
