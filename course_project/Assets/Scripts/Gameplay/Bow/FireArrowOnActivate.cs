using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit; // arriba del todo


public class FireArrowFromRightHandGrip : MonoBehaviour
{
    public XRDirectInteractor rightHandInteractor; 
    public XRInteractionManager interactionManager; 

    public InputActionReference rightGripAction;
    public GameObject arrow;
    public Transform attachArrowPoint;
    public Transform leftHandTransform;
    public Transform spawnPoint;

    public float maxHorizontalDistance = 0.3f;
    public float minPullDistance = 0.05f;
    public float maxPullDistance = 0.5f;
    public float maxArrowSpeed = 15f;

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
                Debug.Log("Flecha creada en mano derecha.");

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
                Vector3 offset = transform.position - leftHandTransform.position;
                Debug.Log($"Offset: {offset}");

                float horizontalDistance = new Vector2(offset.x, offset.y).magnitude;
                float pullDistance = Mathf.Clamp(-offset.z, 0f, maxPullDistance);
                pullDistance = 0.3f; // Valor de prueba para pullDistance

                Debug.Log($"Pull Z: {pullDistance}, Horizontal: {horizontalDistance}");

                if (horizontalDistance <= maxHorizontalDistance && pullDistance >= minPullDistance)
                {
                    float speedFactor = pullDistance / maxPullDistance;
                    float finalSpeed = speedFactor * maxArrowSpeed;

                    currentArrow.transform.parent = null;
                    XRGrabInteractable grabInteractable = currentArrow.GetComponent<XRGrabInteractable>();
                    if (grabInteractable != null)
                    {
                        if (grabInteractable.selectingInteractor == rightHandInteractor)
                        {
                            interactionManager.SelectExit(rightHandInteractor, grabInteractable);
                        }
                    }

                    currentArrow.GetComponent<Rigidbody>().velocity = spawnPoint.forward * finalSpeed;
                    Destroy(currentArrow, 5f);
                    Debug.Log($"Flecha disparada con velocidad: {finalSpeed}");
                }
                else
                {
                    currentArrow.GetComponent<Rigidbody>().velocity = spawnPoint.forward * 0.1f;
                    Destroy(currentArrow, 2f);
                    Debug.Log("Flecha no disparada: manos demasiado separadas.");
                }

                currentArrow = null;
            }
        }

        wasGripping = isGripping;
    }
}
