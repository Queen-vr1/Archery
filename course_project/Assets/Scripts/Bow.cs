using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BowState {
    notArrow, arrowBack, arrowAbove, arrowClose, arrowReady
}

public class Bow : MonoBehaviour
{

    public OVRInput.Button notDominant;
    public GameObject arrowModel;
    private GameObject arrow;
    private BowState currentState = BowState.notArrow;
    public GameObject dominantController, notDominantController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = dominantController.transform.position;
        if (OVRInput.GetDown(OVRInput.Button.One))
            Debug.Log("Dominant controller position: " + dominantController.transform.position);

        if (currentState == BowState.notArrow && OVRInput.GetDown(notDominant) /*&&
            Vector3.Angle(Vector3.down, notDominantController.transform.forward) < 60*/)
        {
            // create the arrow as a child of the notDominant controller
            arrow = Instantiate(arrowModel, notDominantController.transform.position, Quaternion.identity);
            arrow.transform.parent = notDominantController.transform;
            arrow.transform.localPosition = new Vector3(0, 0, 0);
            currentState = BowState.arrowBack;
        }
        else
        if (OVRInput.GetUp(notDominant))
        {
            // give a rigidbody to the arrow so it gets gravity
            Rigidbody rb = arrow.AddComponent<Rigidbody>();
            float distance = Vector3.Distance(notDominantController.transform.position, dominantController.transform.position);
            Vector3 direction = (notDominantController.transform.position - dominantController.transform.position).normalized;
            arrow.transform.parent = null;
            // give a force only if the arrow is ready to shoot
            if (currentState == BowState.arrowReady/* && inBounds(distance)*/)
                    rb.AddForce(direction);
            currentState = BowState.notArrow;
            arrow = null;
        }
        else
        {
            float distance = Vector3.Distance(notDominantController.transform.position, dominantController.transform.position);
            switch (currentState)
            {
                case BowState.arrowBack:
                    Vector3 direction = notDominantController.transform.forward;
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
    }
}
