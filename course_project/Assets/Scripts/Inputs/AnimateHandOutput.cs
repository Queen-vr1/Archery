using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOutput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction; 
    public InputActionProperty gripAnimationAction;
    public Animator handAnimator; 

    private void Start()
    {

    }

    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }
}