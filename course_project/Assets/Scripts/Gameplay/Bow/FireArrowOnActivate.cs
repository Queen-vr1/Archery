using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit; // arriba del todo
using UnityEngine.XR;

public class FireArrowFromRightHandGrip : MonoBehaviour
{
    public XRDirectInteractor rightHandInteractor; 
    public XRInteractionManager interactionManager; 

    public InputActionReference rightGripAction;
    public GameObject arrow;
    public Transform attachArrowPoint;
    public Transform spawnPoint;

    public float maxHorizontalDistance = 0.2f;
    public float minPullDistance = 0.05f;
    public float maxPullDistance = 0.5f;
    public float maxArrowSpeed = 25f;

    private GameObject currentArrow;
    private bool wasGripping = false;

    void Update()
    {
        float gripValue = rightGripAction.action.ReadValue<float>();
        bool isGripping = gripValue > 0.1f;

        // Se acaba de iniciar grip
        if (isGripping && !wasGripping)
        {
            if (currentArrow == null)
            {
                currentArrow = Instantiate(arrow, attachArrowPoint.position, attachArrowPoint.rotation);

                // FORZAR agarre con la mano derecha
                XRGrabInteractable grabInteractable = currentArrow.GetComponent<XRGrabInteractable>();
                if (grabInteractable != null)
                {
                    interactionManager.SelectEnter(rightHandInteractor, grabInteractable);
                }
            }
        }


        // Se acaba de soltar el grip
        if (!isGripping && wasGripping)
        {
            Debug.Log("Soltado el grip");
            if (currentArrow != null)
            {
                Vector3 leftHandPos = InputTracking.GetLocalPosition(XRNode.LeftHand);
                Vector3 rightHandPos = InputTracking.GetLocalPosition(XRNode.RightHand);


                Debug.Log($"[XRNode] Posición mano izquierda: {leftHandPos.x}, {leftHandPos.y}, {leftHandPos.z}");
                Debug.Log($"[XRNode] Posición mano derecha: {rightHandPos.x}, {rightHandPos.y}, {rightHandPos.z}");
                
                float horizontalDiff = Mathf.Abs(rightHandPos.x - leftHandPos.x);
                float tensionZ = leftHandPos.z - rightHandPos.z;
                float pullDistance = Mathf.Clamp(tensionZ, 0f, maxPullDistance);  // solo si tiras hacia adelante

                // pullDistance = 0.3f; // Valor de prueba para pullDistance

                Debug.Log($"Distancia horizontal: {horizontalDiff}, Tensión : {tensionZ}, PullDistance: {pullDistance}");

                if (horizontalDiff  <= maxHorizontalDistance && pullDistance >= minPullDistance)
                {
                    //float speedFactor = pullDistance / maxPullDistance;
                    float speedFactor = Mathf.Pow(tensionZ / maxPullDistance, 2f);
                    float finalSpeed = speedFactor * (maxArrowSpeed + GameManager.Instance.UpgradeState.Arrow_Speed);

					currentArrow.transform.parent = null;
                    XRGrabInteractable grabInteractable = currentArrow.GetComponent<XRGrabInteractable>();
                    if (grabInteractable != null && grabInteractable.selectingInteractor == rightHandInteractor)
                    {
                        interactionManager.SelectExit(rightHandInteractor, grabInteractable);
                    }

                    currentArrow.GetComponent<Rigidbody>().velocity = spawnPoint.forward * finalSpeed;
                    Destroy(currentArrow, 5f);
                    Debug.Log($"Flecha disparada con velocidad: {finalSpeed}");
                }
                else
                {
                    currentArrow.GetComponent<Rigidbody>().velocity = spawnPoint.forward * 0f;
                    Destroy(currentArrow, 2f);
                    Debug.Log("Flecha no disparada: manos demasiado separadas.");
                    Debug.Log($"Distancia horizontal: {horizontalDiff}/{maxHorizontalDistance}, Tensión Z: {pullDistance}/{maxPullDistance}");
                }

                currentArrow = null;
            }
        }

        wasGripping = isGripping;
    }
}
