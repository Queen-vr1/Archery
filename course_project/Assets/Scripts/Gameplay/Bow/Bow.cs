using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;


public enum BowState {
    notArrow, arrowBack, arrowAbove, arrowClose, arrowReady
}

public class Bow : MonoBehaviour
{

    public OVRInput.Button notDominant;
    public GameObject arrowModel;
    private GameObject arrow;
    private BowState currentState = BowState.notArrow;
    private Rigidbody rb;
    public GameObject dominantController, notDominantController;
    public AudioSource bowSound;
    public float mult = 30f;
    public RoundManager roundManager;

    // bowSound.Play(); endiende el sonido 
    // bowSound.Stop(); apaga el sonido

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = notDominantController.transform.position;
        if (OVRInput.GetDown(OVRInput.Button.One))
            Debug.Log("Dominant controller position: " + dominantController.transform.position);

        // Debug.Log ("distance to plane: " + DistanceToPlane(notDominantController.transform.position, Camera.main.transform.position, Camera.main.transform.forward));
        // Debug.Log ("angle: " + Vector3.Angle(Vector3.down, dominantController.transform.forward));

        if (OVRInput.GetDown(notDominant) && currentState == BowState.notArrow && roundManager.timeRemaining > 0
            && DistanceToPlane(dominantController.transform.position, Camera.main.transform.position, Camera.main.transform.forward) < 0.1f
            && GameManager.Instance.ArrowDown())
        {
            arrow = Instantiate(arrowModel, dominantController.transform.position, Quaternion.identity);
            arrow.transform.parent = dominantController.transform;
            // turn off the ribidbody so it doesnt interact
            rb = arrow.GetComponentInChildren<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.detectCollisions = false;
            arrow.transform.localPosition = new Vector3(0, 0, 0);
            arrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("Arrow spawned");
            currentState = BowState.arrowBack;
        }

        else if (OVRInput.GetUp(notDominant) && arrow != null)
        {

            Vector3 direction = Camera.main.transform.forward;
            // give a force only if the arrow is ready to shoot
            // set arrow speed direction to the camera forward
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.detectCollisions = true;
            if (currentState == BowState.arrowReady)
            {
                float distance = Vector3.Distance(dominantController.transform.position, notDominantController.transform.position);
                rb.velocity = arrow.transform.forward * distance * (mult + GameManager.Instance.UpgradeState.Speed_Arrow);
                arrow.GetComponentInChildren<Arrow>().StartCoroutine("DestroyAfter20");
                arrow.transform.parent = null;
                Debug.Log("Arrow shot");
            }
            else if (DistanceToPlane(dominantController.transform.position, Camera.main.transform.position, Camera.main.transform.forward) < 0.1f)
            {
                Destroy(arrow);
                Debug.Log("Arrow returned");
            }
            else
            {
                arrow.transform.parent = null;
                Debug.Log("Arrow fallen");
            }
            bowSound.Stop();
            arrow = null;
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, 0, 1.0f);
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, 0, 1.0f);
            currentState = BowState.notArrow;
        }
        else if (OVRInput.Get(notDominant) && arrow == null && currentState == BowState.notArrow)
        {
            GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");
            foreach (GameObject arr in arrows)
            {
                float distance = Vector3.Distance(dominantController.transform.position, arr.transform.position);
                if (distance < 0.3f)
                {
                    Destroy(arr);
                    GameManager.Instance.ArrowDownWithNoCheck();
                    arrow = Instantiate(arrowModel, dominantController.transform.position, Quaternion.identity);
                    arrow.transform.parent = dominantController.transform;
                    // turn off the ribidbody so it doesnt interact
                    rb = arrow.GetComponentInChildren<Rigidbody>();
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    rb.detectCollisions = false;
                    arrow.transform.localPosition = new Vector3(0, 0, 0);
                    arrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    Debug.Log("Arrow recovered");
                    currentState = BowState.arrowBack;
                }
            }
        }
        else
        {
            float distance = Vector3.Distance(dominantController.transform.position, notDominantController.transform.position);
            switch (currentState)
            {
                case BowState.arrowBack:
                    arrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    Vector3 direction = dominantController.transform.forward;
                    if (direction.y > 0.3) { currentState = BowState.arrowAbove; Debug.Log("Arrow above"); }
                    break;
                case BowState.arrowAbove:
                    arrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (distance < 0.3f) { currentState = BowState.arrowClose; Debug.Log("Arrow close"); }
                    break;
                case BowState.arrowClose:
                    // arrow looks at the nondominant controller
                    arrow.transform.LookAt(notDominantController.transform.position);
                    if (distance >= 0.3f)
                    {
                        bowSound.Play();
                        currentState = BowState.arrowReady;
                        Debug.Log("Arrow ready");
                    }
                    break;
                case BowState.arrowReady:
                    arrow.transform.LookAt(notDominantController.transform.position);
                    // controllers vibrate
                    float vibration = 0.2f + (distance - 0.3f) * 1.6f;
                    InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, vibration, 1.0f);
                    InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, vibration, 1.0f);
                    if (distance < 0.3f)
                    {
                        bowSound.Stop();
                        currentState = BowState.arrowClose;
                        Debug.Log("Arrow close again");
                        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, 0f, 1.0f);
                        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, 0f, 1.0f);
                    }
                    if (distance > 0.8f)
                    {
                        bowSound.Stop();
                        currentState = BowState.arrowAbove;
                        Debug.Log("Arrow back");
                        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, 0f, 1.0f);
                        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0, 0f, 1.0f);
                    }
                    break;
            }
        }

        // if (OVRInput.Get(notDominant)) {
        //     arrow.transform.localRotation = Quaternion.Euler(0, -90, 0);
        // }
        /*if (currentState == BowState.notArrow && OVRInput.GetDown(notDominant)
            && DistanceToPlane(notDominantController.transform.position, Camera.main.transform.position, Camera.main.transform.forward) < 0f
            && Vector3.Angle(Vector3.down, dominantController.transform.forward) < 60);*/

/*
        if (currentState == BowState.notArrow && OVRInput.GetDown(notDominant)
            && Vector3.Angle(Vector3.down, dominantController.transform.forward) < 60)
        {
            // create the arrow as a child of the notDominant controller
            arrow = Instantiate(arrowModel, dominantController.transform.position, Quaternion.identity);
            arrow.transform.parent = dominantController.transform;
            arrow.transform.localPosition = new Vector3(0, 0, 0);
            currentState = BowState.arrowBack;
        }
        else
        if (OVRInput.GetUp(notDominant))
        {
            // give a rigidbody to the arrow so it gets gravity
            Rigidbody rb = arrow.AddComponent<Rigidbody>();
            float distance = Vector3.Distance(dominantController.transform.position, notDominantController.transform.position);
            Vector3 direction = (dominantController.transform.position - notDominantController.transform.position).normalized;
            arrow.transform.parent = null;
            // give a force only if the arrow is ready to shoot
            // if (currentState == BowState.arrowReady && inBounds(distance))
            if (currentState == BowState.arrowReady)
                    rb.AddForce(direction);
            currentState = BowState.notArrow;
            arrow = null;
        }
        else
        {
            float distance = Vector3.Distance(dominantController.transform.position, notDominantController.transform.position);
            switch (currentState)
            {
                case BowState.arrowBack:
                    Vector3 direction = dominantController.transform.forward;
                    if (direction.y > 0.3) currentState = BowState.arrowAbove;
                    break;
                case BowState.arrowAbove:
                    if (distance < 0.2f) currentState = BowState.arrowClose;
                    break;
                case BowState.arrowClose:
                    if (distance > 0.4f) currentState = BowState.arrowReady;
                    break;
                // case BowState.arrowReady:
                //     if (distance > 0.8f) currentState = BowState.arrowReady;
                //     break;
            }
        }
        */
    }

    private float DistanceToPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
    {
        Vector3 pointToPlane = point - planePoint;
        return Vector3.Dot(pointToPlane, planeNormal);
    }
}
