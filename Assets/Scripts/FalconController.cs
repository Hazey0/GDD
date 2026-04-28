using UnityEngine;
using UnityEngine.InputSystem;

public class FalconController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float verticalSpeed = 6f;
    public float boostSpeed = 16f;

    [Header("Movement Smoothing")]
    public float rotationSpeed = 10f;

    private Vector2 moveInput;
    private float verticalInput;

    private bool isActiveCharacter = false;
    private bool isBoosting = false;

    private void Update()
    {
        if (!isActiveCharacter)
            return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 horizontalMove = new Vector3(moveInput.x, 0f, moveInput.y);
        horizontalMove = Vector3.ClampMagnitude(horizontalMove, 1f);

        Vector3 verticalMove = Vector3.up * verticalInput;

        Vector3 finalMoveDirection = horizontalMove + verticalMove;
        finalMoveDirection = Vector3.ClampMagnitude(finalMoveDirection, 1f);

        float currentSpeed = isBoosting ? boostSpeed : moveSpeed;

        transform.position += finalMoveDirection * currentSpeed * Time.deltaTime;

        if (horizontalMove.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalMove);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void SetActiveCharacter(bool active)
    {
        isActiveCharacter = active;

        if (!active)
        {
            moveInput = Vector2.zero;
            verticalInput = 0f;
            isBoosting = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnFlyVertical(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;

        verticalInput = context.ReadValue<float>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;

        if (context.performed)
        {
            isBoosting = true;
        }

        if (context.canceled)
        {
            isBoosting = false;
        }
    }
}