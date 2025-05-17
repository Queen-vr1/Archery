using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandUIRay : MonoBehaviour
{
    [Header("References")]
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;
    public LineRenderer rayRenderer;
    public float maxDistance = 10f;
    public float uiInteractionDistance = 2f;
    
    [Header("Visual Settings")]
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.blue;
    public float rayWidth = 0.01f;
    
    private Transform controllerTransform;
    private bool isHoveringUI = false;
    private GameObject currentUIObj;

    void Start()
    {
        // Configurar LineRenderer
        if (rayRenderer == null)
        {
            rayRenderer = gameObject.AddComponent<LineRenderer>();
            rayRenderer.material = new Material(Shader.Find("Unlit/Color"));
            rayRenderer.material.color = defaultColor;
            rayRenderer.startWidth = rayWidth;
            rayRenderer.endWidth = rayWidth;
        }
        
        // Obtener referencia al transform del controlador
        controllerTransform = (controller == OVRInput.Controller.LTouch) ? 
            GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").transform : 
            GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor").transform;
    }

    void Update()
    {
        UpdateRay();
        HandleUIInteraction();
    }

    void UpdateRay()
    {
        // Actualizar posición y rotación del rayo
        rayRenderer.SetPosition(0, controllerTransform.position);
        
        // Comprobar interacción con UI
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2);
        
        // Raycast contra UI
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        
        float rayDistance = maxDistance;
        isHoveringUI = false;
        
        if (results.Count > 0)
        {
            foreach (var result in results)
            {
                if (result.distance < uiInteractionDistance)
                {
                    rayDistance = result.distance;
                    isHoveringUI = true;
                    currentUIObj = result.gameObject;
                    break;
                }
            }
        }
        
        // Calcular punto final del rayo
        Vector3 endPoint = controllerTransform.position + controllerTransform.forward * rayDistance;
        rayRenderer.SetPosition(1, endPoint);
        
        // Cambiar color si está sobre UI
        rayRenderer.material.color = isHoveringUI ? hoverColor : defaultColor;
    }

    void HandleUIInteraction()
    {
        if (!isHoveringUI || currentUIObj == null) return;
        
        // Detectar clic en UI
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            ExecuteEvents.Execute(currentUIObj, new PointerEventData(EventSystem.current), 
                ExecuteEvents.pointerClickHandler);
        }
        
        // Detectar hover (opcional)
        ExecuteEvents.Execute(currentUIObj, new PointerEventData(EventSystem.current), 
            ExecuteEvents.pointerEnterHandler);
    }
}