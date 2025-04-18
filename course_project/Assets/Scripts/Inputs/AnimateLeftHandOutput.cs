using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateLeftHandOutput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction; 
    //public InputActionProperty gripAnimationAction;
    public Animator handAnimator; 
    [Range(0f, 1f)] public float gripAmount = 1f;

    private void Start()
    {

    }

    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);

        // float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripAmount);
    }
}