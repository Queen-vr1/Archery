using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TeleportController : MonoBehaviour
{
    [Header("References")]
    public OVRCameraRig cameraRig;
    public Transform leftHandAnchor;
    public Transform rightHandAnchor;
    public GameObject teleportMarkerPrefab;
    
    [Header("Settings")]
    public float maxTeleportDistance = 10f;
    public LayerMask teleportableLayers;
    public string allowedTeleportTag = "Teleportable"; // Nuevo: Tag requerido
    public float parabolaAngle = 45f;
    public int lineSegmentCount = 20;
    public Color validTeleportColor = Color.green;
    public Color invalidTeleportColor = Color.red;
    
    private LineRenderer teleportLine;
    private GameObject teleportMarker;
    private bool isAiming = false;
    private Transform currentController;
    private Vector3? teleportTarget;
    private bool isValidTeleport = false; // Nuevo: Estado de validez

    void Start()
    {
        teleportLine = GetComponent<LineRenderer>();
        teleportLine.enabled = false;
        teleportLine.positionCount = lineSegmentCount;
        teleportLine.useWorldSpace = true;
        
        teleportMarker = Instantiate(teleportMarkerPrefab);
        teleportMarker.SetActive(false);
    }

    void Update()
    {
        HandleInput();
        if (isAiming)
        {
            UpdateTeleportArc();
        }
    }

    void HandleInput()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            StartAiming(rightHandAnchor);
        }
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            StartAiming(leftHandAnchor);
        }
        
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && 
            currentController == rightHandAnchor ||
            (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch) && 
            currentController == leftHandAnchor))
        {
            if (isValidTeleport) // Solo teleportar si es válido
            {
                TryTeleport();
            }
            StopAiming();
        }
    }

    void StartAiming(Transform controller)
    {
        isAiming = true;
        currentController = controller;
        teleportLine.enabled = true;
        isValidTeleport = false; // Resetear estado al comenzar
    }

    void StopAiming()
    {
        isAiming = false;
        teleportLine.enabled = false;
        teleportMarker.SetActive(false);
    }

    void UpdateTeleportArc()
    {
        Vector3 velocity = currentController.forward * maxTeleportDistance;
        velocity = Quaternion.AngleAxis(-parabolaAngle, currentController.right) * velocity;
        
        Vector3[] points = new Vector3[lineSegmentCount];
        points[0] = currentController.position;
        teleportTarget = null;
        isValidTeleport = false;
        
        bool hitDetected = false;
        Vector3 hitPoint = Vector3.zero;

        for (int i = 1; i < lineSegmentCount; i++)
        {
            float t = (float)i / (lineSegmentCount - 1);
            Vector3 gravity = Physics.gravity * t * t;
            
            // Si ya hubo un impacto, mantenemos la posición del impacto
            if (hitDetected)
            {
                points[i] = hitPoint;
                continue;
            }
            
            // Calcular nueva posición
            points[i] = points[0] + velocity * t + 0.5f * gravity;
            
            // Detectar colisión
            if (i > 0 && Physics.Linecast(points[i-1], points[i], out RaycastHit hit, teleportableLayers))
            {
                hitDetected = true;
                hitPoint = hit.point;
                
                // Verificar tag del objeto impactado
                if (hit.collider.CompareTag(allowedTeleportTag))
                {
                    teleportTarget = hit.point;
                    teleportMarker.SetActive(true);
                    teleportMarker.transform.position = hit.point + hit.normal * 0.1f;
                    teleportMarker.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    isValidTeleport = true;
                }
                else
                {
                    // Objeto no válido - mostrar hasta el punto de impacto
                    isValidTeleport = false;
                    teleportMarker.SetActive(false);
                }
                
                // Ajustar el punto actual al punto de impacto
                points[i] = hitPoint;
            }
        }
        
        // Actualizar colores y posiciones del LineRenderer
        teleportLine.startColor = isValidTeleport ? validTeleportColor : invalidTeleportColor;
        teleportLine.endColor = isValidTeleport ? validTeleportColor : invalidTeleportColor;
        teleportLine.SetPositions(points);
    }

    void TryTeleport()
    {
        if (teleportTarget.HasValue && isValidTeleport)
        {
            Vector3 headPosition = cameraRig.centerEyeAnchor.position;
            Vector3 difference = headPosition - cameraRig.transform.position;
            difference.y = 0;
            
            cameraRig.transform.position = teleportTarget.Value - difference;
        }
    }
}