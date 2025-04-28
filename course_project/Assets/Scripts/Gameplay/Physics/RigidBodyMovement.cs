using UnityEngine;
using UnityEngine.InputSystem; // Usar esto si estás usando el nuevo Input System

public class XRRigidBodyMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 3f;
    public float jumpForce = 5f;
    public InputActionProperty moveInput;
    public InputActionProperty jumpInput;

    private bool isGrounded;

    private void Update()
    {
        // Saltar
        if (jumpInput.action.WasPressedThisFrame() && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Movimiento
        Vector2 input = moveInput.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        // Mover relativo a la dirección del jugador (puedes usar la dirección de la cámara si quieres)
        Vector3 moveDirection = transform.TransformDirection(move);

        Vector3 targetPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Detectar si estamos en el suelo
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        // No estamos en el suelo
        isGrounded = false;
    }
}
