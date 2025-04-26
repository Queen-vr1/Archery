using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateTeleportationRay : MonoBehaviour
{
    public InputActionProperty rightActivate; 
    public GameObject rightTeleportation; 

	  //public XRRayInteractor rightRay;
	private void Start()
    {
    }

    void Update()
    {
		bool isRightRayHovering = false;// rightRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid);
		rightTeleportation.SetActive(!isRightRayHovering && rightActivate.action.ReadValue<float>() > 0.1f);
    }
}
