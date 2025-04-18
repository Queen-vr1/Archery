using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateTeleportationRay : MonoBehaviour
{
    public InputActionProperty rightActivate; 
    public GameObject rightTeleportation; 

    private void Start()
    {
    }

    void Update()
    {
        rightTeleportation.SetActive(rightActivate.action.ReadValue<float>() > 0.1f);
    }
}